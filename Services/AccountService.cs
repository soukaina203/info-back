using Microsoft.AspNetCore.Mvc;
using Models;           // Make sure you have a User or Account model in here
using context;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Security.Claims;          // This should be your actual namespace (e.g., MyApp.Data)
using DTO;
using Services;
namespace Services
{

	public class AccountService
	{
		private readonly PasswordHasher _passwordHasher;
		private readonly AppDbContext _context;
		private readonly JwtService _jwtService;

		public AccountService(PasswordHasher passwordHasher, AppDbContext context, JwtService jwtService)
		{
			_passwordHasher = passwordHasher;
			_context = context;
			_jwtService = jwtService;

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
				await _context.Users.AddAsync(user);
				await _context.SaveChangesAsync();

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
			try
			{
				await _context.ProfProfiles.AddAsync(user.ProfProfile);
				await _context.SaveChangesAsync();

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
