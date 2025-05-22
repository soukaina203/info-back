using Microsoft.AspNetCore.Mvc;
using Models;           // Make sure you have a User or Account model in here
using context;          // This should be your actual namespace (e.g., MyApp.Data)
using Services;
using Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using DTO;
using Enums;

namespace Controllers
{
	[ApiController]
	[Route("api/[controller]/[action]")]
	public class AccountController : ControllerBase
	{
		private readonly AppDbContext _context;
		private readonly AccountService _accountService;
		private readonly EmailService _emailService;
		private readonly JwtService _jwtService;

		

		public AccountController(AppDbContext context, AccountService accountService,EmailService emailService, JwtService jwtService)
		{
			_context = context;
			_jwtService = jwtService;
			_accountService = accountService;
			_emailService=emailService;
		}


		[HttpPost]
		public async Task<IActionResult> Login(LoginDTO user)
		{
			var result = await _accountService.Login(user);

			// Si login réussi, on ajoute le refresh token dans le cookie
			if (result.Code == 1 && !string.IsNullOrEmpty(result.RefreshToken))
			{
				Response.Cookies.Append("refreshToken", result.RefreshToken, new CookieOptions
				{
					HttpOnly = true,
					Secure = true,
					SameSite = SameSiteMode.Strict,
					Expires = DateTime.UtcNow.AddDays(30)
				});

				// On peut retourner sans le refreshToken dans le JSON (si tu veux le cacher côté frontend)
				result.RefreshToken = null;
			}

			return Ok(result);
		}


		[HttpGet("{userId}")]
		public async Task<IActionResult> ActiveAccount(int userId)
		{
			var user = await _context.Users.FirstOrDefaultAsync(e => e.Id == userId);

			if (user == null)
			{
				return NotFound("User not found.");
			}

			user.Status = VerificationStatus.Verified;
			_context.Users.Update(user);
			await _context.SaveChangesAsync();

			return Ok(new{Code = 1 ,Message="success"});
		}

		
		[HttpPost]
		public async Task<IActionResult> RegisterStudent(User user)
		{
			var result = await _accountService.RegisterUser(user);

	
				return Ok(result);

		}
		
		
		[HttpPost]
		public async Task<IActionResult> RegisterProf(ProfInscriptionDTO user)
		{
			var result = await _accountService.RegisterTeacher(user);

				return Ok(result);

		}
		


		[HttpGet]
		public async Task<IActionResult> GetServicesData()
		{
			var services = await _context.Services.ToListAsync();
			var specialities = await _context.Specialities.ToListAsync();
			var niveaux = await _context.Niveaux.ToListAsync();
			var methods = await _context.Methods.ToListAsync();
			return Ok(new { services = services, specialities = specialities, niveaux = niveaux, methods = methods });
		}
		
		
		[HttpGet(	)]
		public async Task<IActionResult> Refresh()
		{
			var refreshToken = Request.Cookies["refreshToken"];
			if (string.IsNullOrEmpty(refreshToken)) return Unauthorized();

			var user = await _context.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);

			if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
				return Unauthorized();

			var newAccessToken = _jwtService.GenerateToken(user.Id.ToString(), user.Email);

			return Ok(new { token = newAccessToken });
		}

	}
}	
