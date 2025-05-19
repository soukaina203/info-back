using Models;
using context;
using Microsoft.EntityFrameworkCore;
using DTO;
using Microsoft.AspNetCore.Mvc;
using Services;
namespace Services
{

	public class AccountService
	{
		private readonly PasswordHashing _passwordHasher;
		private readonly JwtService _jwtService;
		private readonly EmailService _emailService;

		private readonly AppDbContext _context;
		public AccountService(PasswordHashing passwordHasher, AppDbContext context, JwtService jwtService,EmailService emailService)
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

				if (!user.Status)
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

				_context.Users.Update(user);
				await _context.SaveChangesAsync();

				return new loginResponse
				{
					Message = "Successfull login",
					Code = 1,
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
				user.IsAdmin=false;
				
				 await _context.Users.AddAsync(user);
				 await _context.SaveChangesAsync();
				var registrerdUser =await _context.Users.Where(u=>u.Email== user.Email).FirstOrDefaultAsync();
				var token = _jwtService.GenerateToken(registrerdUser.Id.ToString(), user.Email); // problem because id is 0 
				var name= user.FirstName + " " + user.LastName;
				var emailSendingResult=await _emailService.SendVerificationEmailAsync(user.Email,name ,token); // Done 
				if (emailSendingResult!=true)
				{
				return new RegistrationResponse<string> { Code = 1, Message = "Email not sent", Token = token, UserId = user.Id ,IsEmailSended = false  };
				}
				return new RegistrationResponse<string> { Code = 1, Message = "Register Successful", Token = token, UserId = user.Id ,IsEmailSended = true, User=user  };
			}
			catch (DbUpdateConcurrencyException ex)
			{
				return new RegistrationResponse<string> { Code = -2, Message = ex.Message };
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

				return new RegistrationResponse<string> { Code = 1, Message = "Teacher registered successfully", Token = userRegistrationResult.Token , UserId = userRegistrationResult.UserId };
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
