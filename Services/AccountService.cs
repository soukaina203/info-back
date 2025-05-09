using Models;
using context;
using Microsoft.EntityFrameworkCore;
using DTO;
using Microsoft.AspNetCore.Mvc;

namespace Services
{

	public class AccountService
	{
		private readonly PasswordHasher _passwordHasher;
		private readonly JwtService _jwtService;

		private readonly AppDbContext _context;
		public AccountService(PasswordHasher passwordHasher, AppDbContext context, JwtService jwtService)
		{
			_passwordHasher = passwordHasher;
			_context = context;
			_jwtService = jwtService;

		}

		public async Task<loginResponse> Login(LoginDTO model)
		{
			if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
			{
				return new loginResponse { Code = -4, Message = "Email | password required" };
			}
			var user = await _context.Users.Where(x => x.Email == model.Email).AsNoTracking().FirstOrDefaultAsync();
			if (user == null)
			{
				return new loginResponse { Message = "Email error", Code = -3 };

			}

			var newHash = _passwordHasher.HashPassword(model.Password);
            if (newHash != user.Password)
			{
				return new loginResponse { Message = "Error Password", Code = -1 };
			}

			var accessToken = _jwtService.GenerateToken(user.Id.ToString(), user.Email);
			var refreshToken = _jwtService.GenerateRefreshToken();
			user.RefreshToken = refreshToken;
			user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
			_context.Users.Update(user);
			await _context.SaveChangesAsync();

			return new loginResponse { Message = "Successfull login", Code = 1 ,Token=accessToken};
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
				_ = await _context.Users.AddAsync(user);
				_ = await _context.SaveChangesAsync();

				var token = _jwtService.GenerateToken(user.Id.ToString(), user.Email);
				return new RegistrationResponse<string> { Code = 1, Message = "Register Successful", Data = token, UserId = user.Id };
			}
			catch (DbUpdateConcurrencyException ex)
			{
				return new RegistrationResponse<string> { Code = -2, Message = ex.Message };
			}
		}

		public async Task<RegistrationResponse<string>> RegisterTeacher(ProfInscriptionDTO user)
		{
			// ProfInscription DTO = USER , Prof
			var userRegistrationResult = await RegisterUser(user.User);
			if (userRegistrationResult.Code != 1 || userRegistrationResult.UserId == 0)
			{
				return userRegistrationResult; // failed user registration
			}
			user.ProfProfile.UserId = userRegistrationResult.UserId;
			var registeredUser = await _context.Users.FindAsync(userRegistrationResult.UserId);
			if (registeredUser != null)
			{
				user.ProfProfile.User = registeredUser;
			}

			try
			{
				_ = await _context.ProfProfiles.AddAsync(user.ProfProfile);
				_ = await _context.SaveChangesAsync();

				return new RegistrationResponse<string> { Code = 1, Message = "Teacher registered successfully" };
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
