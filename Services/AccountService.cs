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

		public async Task<ServiceResponse<string>> RegisterUser(User user)
		{
			var emailExiste = await _context.Users.FirstOrDefaultAsync(e => e.Email == user.Email);
			if (emailExiste != null)
			{
				return new ServiceResponse<string> { Code = -1, Message = "Email already exists" };
			}

			user.Password = _passwordHasher.HashPassword(user.Password);
			try
			{
				await _context.Users.AddAsync(user);
				await _context.SaveChangesAsync();

				var token = _jwtService.GenerateToken(user.Id.ToString(), user.Email);
				return new ServiceResponse<string> { Code = 1, Message = "Register Successful", Data = token };
			}
			catch (DbUpdateConcurrencyException ex)
			{
				return new ServiceResponse<string> { Code = -2, Message = ex.Message };
			}
		}
		public bool CheckPassword(string enteredPassword, string storedHashedPassword)
		{
			return _passwordHasher.VerifyPassword(enteredPassword, storedHashedPassword);
		}
		
		
	}
}
