using Microsoft.AspNetCore.Mvc;
using Models;               // Contient probablement l'entité User ou Account
using context;              // Namespace pour la base de données (ex. : MyApp.Data)
using Services;             // Contient les services métier
using Utilities;            // Pour les utilitaires comme le JWT
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using DTO;                  // Objets de transfert de données
using Enums;                // Pour les énumérations comme VerificationStatus

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

		// Injection des dépendances dans le constructeur
		public AccountController(AppDbContext context, AccountService accountService, EmailService emailService, JwtService jwtService)
		{
			_context = context;
			_accountService = accountService;
			_emailService = emailService;
			_jwtService = jwtService;
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginDTO user)
		{
			var result = await _accountService.Login(user);

			// Si login réussi, on ajoute le refresh token dans le cookie sécurisé
			if (result.Code == 1 && !string.IsNullOrEmpty(result.RefreshToken))
			{
				Response.Cookies.Append("refreshToken", result.RefreshToken, new CookieOptions
				{
					HttpOnly = true,
					Secure = true,
					SameSite = SameSiteMode.None,
					Expires = DateTime.UtcNow.AddDays(30)
				});
			}

			return Ok(result);
		}

		[HttpPost]
		public async Task<IActionResult> VerifyRegistrationToken(VerifyRegistrationTokenDTO userData)
		{
			var result = await _accountService.VerifyRegistrationToken(userData);
			return Ok(result);
		}

		[HttpPost]
		public async Task<IActionResult> ResetPassword(ResetPwdDTO userData)
		{
			var result = await _accountService.ResetPassword(userData);
			return Ok(result);
		}

		[HttpGet("{email}")]
		public async Task<IActionResult> ForgetPassword(string email)
		{
			var result = await _accountService.ForgetPassword(email);
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

			// Mise à jour du statut de vérification
			user.Status = VerificationStatus.Verified;
			_context.Users.Update(user);
			await _context.SaveChangesAsync();

			return Ok(new { Code = 1, Message = "success" });
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
			// Chargement des données nécessaires à l’inscription (services, niveaux, etc.)
			var services = await _context.Services.ToListAsync();
			var specialities = await _context.Specialities.ToListAsync();
			var niveaux = await _context.Niveaux.ToListAsync();
			var methods = await _context.Methods.ToListAsync();

			return Ok(new
			{
				services = services,
				specialities = specialities,
				niveaux = niveaux,
				methods = methods
			});
		}

		[HttpGet("{userId}")]
		public async Task<IActionResult> Refresh(int userId)
		{

			var user = _context.Users.Where(u => u.Id == userId).FirstOrDefault();

			if (user==null)
				return Unauthorized();


			if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
				return Unauthorized();

			var newAccessToken = _jwtService.GenerateToken(user.Id.ToString(), user.Email);

			return Ok(new { token = newAccessToken });
		}
	}
}
