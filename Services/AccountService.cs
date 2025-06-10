using Models;
using context;
using Microsoft.EntityFrameworkCore;
using DTO;
using Microsoft.AspNetCore.Mvc;
using Utilities;
using Enums;
using System.Security.Claims;

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
		
		public async Task<ResponseDTO> VerifyRegistrationToken(VerifyRegistrationTokenDTO userData)
		{
			// 1. Validate token
			var principal = _jwtService.ValidateToken(userData.Token);
			if (principal == null)
			{
				return new ResponseDTO { Success = false, Message = "Invalid token" };
			}

			// 2. Extract and validate claims
			var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			var email = principal.FindFirst(ClaimTypes.Email)?.Value;

			if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(email))
			{
				return new ResponseDTO { Success = false, Message = "Token missing required claims" };
			}

			// 3. Parse user ID safely
			if (!int.TryParse(userId, out int globalUserIdInt))
			{
				return new ResponseDTO { Success = false, Message = "Invalid user ID format in token" };
			}

			// 4. Verify token belongs to the current user
			if (userData.Id != globalUserIdInt)
			{
				return new ResponseDTO { Success = false, Message = "This token does not belong to the current user" };
			}

			return new ResponseDTO { Success = true };
		}
		
		
		
		public async Task<ResponseDTO> ResetPassword(ResetPwdDTO userData)
		{
			
			// 1. Validate the token
			var principal = _jwtService.ValidateToken(userData.Token);
			if (principal == null)
			{
				return new ResponseDTO { Success = false, Message = "Invalid token" };
			}

			// 2. Extract claims
			var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			var email = principal.FindFirst(ClaimTypes.Email)?.Value;

			if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(email))
			{
				return new ResponseDTO { Success = false, Message = "Token missing required claims" };
			}

		
			try
			{
			// Parse the userId first
			if (!int.TryParse(userId, out int parsedUserId))
			{
				return new ResponseDTO { Success = false, Message = "Invalid user ID format" };
			}

			// Then use it in the query with proper lambda syntax
			var user = await _context.Users
				.FirstOrDefaultAsync(u => u.Id == parsedUserId); // Correct lambda expression		
				if (user==null)
				
				{
					return new ResponseDTO { Success = false, Message = "User not found" };
				}

				// Reset password
				var result = await SetNewPassword(parsedUserId, userData.Password);

				if (result.Success!=true)
				{
					return new ResponseDTO 
					{ 
						Success = false, 
						Message = "Password reset failed "
					};
				}

				return new ResponseDTO { Success = true, Message = "Password reset successfully" };
			}
			catch (Exception ex)
			{
				return new ResponseDTO { Success = false, Message = $"Error: {ex.Message}" };
			}
		}



		public async Task<ResponseDTO> SetNewPassword(int userId , string password)

		{
			var user =await _context.Users.Where(u=>u.Id==userId).FirstOrDefaultAsync();
			
			if (user==null)
			{
				return new ResponseDTO { Success = false, Message = "user not found" };
			}
  			  user.Password = _passwordHasher.HashPassword(password);
			_context.Entry(user).State = EntityState.Modified;
			await _context.SaveChangesAsync();
				return new ResponseDTO { Success = true, Message = "pqssword successfully changed" };
				
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



		public async Task<ResponseDTO> ForgetPassword(ForgetPwdDTO userData)
		{
			try
			{
				
				var IsEmailExists= await _context.Users.Where(u=>u.Email==userData.Email).FirstOrDefaultAsync();
				// Générer le token
				if (IsEmailExists==null)
				{
						return new ResponseDTO  {
						Code = 404,
						Message = "L'e-mail n'existe pas"
					};
				}
				var token = _jwtService.GenerateToken(userData.Id.ToString(), userData.Email);

				// Envoyer l'e-mail
				var emailSendingResult = await _emailService.SendVerificationEmailAsync(
					userData.Email,
					userData.Name,
					token,
					"ResetPwdEmail"
				);

				// Vérifier le résultat de l'envoi
				if (emailSendingResult.Success)
				{
				
					return new ResponseDTO  {
						Code = 200,
						Message = "L'e-mail de réinitialisation a été envoyé avec succès."
					};
				}
				else
				{
				
					return new ResponseDTO {
						Code = 500,
						Message = "Échec de l'envoi de l'e-mail de réinitialisation."
					};
				}
			}
			catch (Exception ex)
			{
		
				return new ResponseDTO {
					Code = 500,
					Message = $"Une erreur est survenue : {ex.Message}"
				};
			}

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

				var emailSendingResult = await _emailService.SendVerificationEmailAsync(user.Email, name, token,"EmailVerification"); // Done 
				if (emailSendingResult.Success != true)
				{
					registrerdUser.Status = VerificationStatus.Failed; // before the mail sending
					_context.Entry(registrerdUser).State = EntityState.Modified;
					await _context.SaveChangesAsync();

					
					return new RegistrationResponse<string>
					{
						Code = 1,
						Message = "Email not sent",
						Token = token,
						UserId = user.Id,
						IsEmailSended = false,
						errors = emailSendingResult.ErrorMessage
					};
				}
				// if the mail is sent
				registrerdUser.Status = VerificationStatus.EmailSended; // before the mail sending
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
				return new RegistrationResponse<string> { Code = 1, Message = "Register Successful", Token = token,UserData = userData ,UserId = user.Id, IsEmailSended = true, User = user };
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
