using Models;
using context;
using Microsoft.EntityFrameworkCore;
using DTO;
using Microsoft.AspNetCore.Mvc;
using Utilities;
using Enums;
namespace Services
{

	public class AccountService
	{
		private readonly PasswordHashing _passwordHasher;
		private readonly JwtService _jwtService;
		private readonly EmailService _emailService;

		private readonly AppDbContext _context;
		public AccountService(PasswordHashing passwordHasher, AppDbContext context, JwtService jwtService, EmailService emailService)
		{
			_passwordHasher = passwordHasher;
			_context = context;
			_jwtService = jwtService;
			_emailService = emailService;
		}

		public async Task<loginResponse> Login(LoginDTO model)
		{
			if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
			{
				return new loginResponse { Code = -4, Message = "Email | password required" };
			}

			var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == model.Email);
			if (user == null)
			{
				return new loginResponse { Message = "Email error", Code = -3 };
			}

			if (user.Status != VerificationStatus.Verified)
			{
				return new loginResponse
				{
					Message = "Compte non activé. Veuillez consulter votre boîte mail.",
					Code = -4
				};
			}

			if (!CheckPassword(model.Password, user.Password))
			{
				return new loginResponse { Message = "Mot de passe incorrect", Code = -1 };
			}

			var accessToken = _jwtService.GenerateToken(user.Id.ToString(), user.Email);
			var refreshToken = _jwtService.GenerateRefreshToken();

			user.RefreshToken = refreshToken;
			user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(30);

			var userData = new UserDto
			{
				Id = user.Id,
				LastName = user.LastName,
				FirstName = user.FirstName,
				Email = user.Email,
				Photo = user.Photo,
				Telephone = user.Telephone,
				RoleId = user.RoleId
			};
		

			_context.Users.Update(user);
			await _context.SaveChangesAsync();

			return new loginResponse
			{
				Message = "Successfull login",
				Code = 1,
				UserId = user.Id,
				UserData = userData,
				Token = accessToken,
				RefreshToken = refreshToken
			};
		}


		public async Task<RegistrationResponse<string>> RegisterUser(User user)
		{
			var emailExiste = await _context.Users.FirstOrDefaultAsync(e => e.Email == user.Email);
			if (emailExiste != null)
			{
				return new RegistrationResponse<string> { Code = -1, Message = "Email already exists" };
			}
			user.Password = _passwordHasher.HashPassword(user.Password);
			try
			{
				// the user regitration should be always pending in the status until the email is send 
				// then the status goes to false until the email is verified 
				// then to true when the user verify the email
				user.Status = VerificationStatus.Pending; // before the mail sending
				await _context.Users.AddAsync(user);
				await _context.SaveChangesAsync();

				var registrerdUser = await _context.Users.Where(u => u.Email == user.Email).FirstOrDefaultAsync(); // l utilisateur doit etre deja enregistre dans la base de donnees
				if (registrerdUser == null)
				{
					return new RegistrationResponse<string> { Code = -4, Message = "failed to register the user " };
				}
				var token = _jwtService.GenerateToken(registrerdUser.Id.ToString(), user.Email); // on doit utiliser l'id car il est inchangable

				var name = user.FirstName + " " + user.LastName;

				var emailSendingResult = await _emailService.SendVerificationEmailAsync(user.Email, name, token); // Done 
				if (emailSendingResult.Success != true)
				{
					registrerdUser.Status = VerificationStatus.Failed; // before the mail sending
					_context.Entry(registrerdUser).State = EntityState.Modified;
					await _context.SaveChangesAsync();

					var userData = new UserDto
					{
						Id = user.Id,
						LastName = user.LastName,
						FirstName = user.FirstName,
						Email = user.Email,
						Photo = user.Photo,
						Telephone = user.Telephone,
						RoleId = user.RoleId
					};
					return new RegistrationResponse<string>
					{
						Code = 1,
						Message = "Email not sent",
						Token = token,
						UserId = user.Id,
						UserData = userData,
						IsEmailSended = false,
						errors = emailSendingResult.ErrorMessage
					};
				}
				// if the mail is sent
				registrerdUser.Status = VerificationStatus.EmailSended; // before the mail sending
				_context.Entry(registrerdUser).State = EntityState.Modified;
				await _context.SaveChangesAsync();


				return new RegistrationResponse<string> { Code = 1, Message = "Register Successful", Token = token, UserId = user.Id, IsEmailSended = true, User = user };
			}
			catch (DbUpdateConcurrencyException ex)

			{

				return new RegistrationResponse<string>
				{
					Code = -2,
					Message = ex.Message
				};
			}
		}



		public async Task<RegistrationResponse<string>> RegisterTeacher(ProfInscriptionDTO user)
		{
			var userRegistrationResult = await RegisterUser(user.user);
			if (userRegistrationResult.Code != 1 || userRegistrationResult.UserId == 0)
			{
				return new RegistrationResponse<string> { Code = 1, Message = "Something went wrong in RegisterUser" };
			}
			user.profProfile.UserId = userRegistrationResult.UserId;
			var registeredUser = await _context.Users.FindAsync(userRegistrationResult.UserId);
			if (registeredUser != null)
			{
				user.profProfile.User = registeredUser;
			}

			try
			{
				await _context.ProfProfiles.AddAsync(user.profProfile);
				await _context.SaveChangesAsync();

				return new RegistrationResponse<string>
				{
					Code = 1,
					Message = "Teacher registered successfully",
					Token = userRegistrationResult.Token,
					UserId = userRegistrationResult.UserId
				};
			}
			catch (DbUpdateException ex)
			{
				return new RegistrationResponse<string> { Code = -3, Message = "Teacher profile creation failed: " + ex.Message };
			}

		}





		public bool CheckPassword(string enteredPassword, string storedHashedPassword)
		{
			return _passwordHasher.VerifyPassword(enteredPassword, storedHashedPassword);
		}


	}
}
