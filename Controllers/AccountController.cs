using Microsoft.AspNetCore.Mvc;
using Models;           // Make sure you have a User or Account model in here
using context;          // This should be your actual namespace (e.g., MyApp.Data)
using Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using DTO;

namespace Controllers
{
	[ApiController]
	[Route("api/[controller]/[action]")]
	public class AccountController : ControllerBase
	{
		private readonly AppDbContext _context;
		private readonly AccountService _accountService;
		private readonly EmailService _emailService;

		public AccountController(AppDbContext context, AccountService accountService,EmailService emailService)
		{
			_context = context;
			_accountService = accountService;
			_emailService=emailService;
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginDTO user)
		{
			var result = await _accountService.Login(user);

			if (result.Code == 1 || result.Code != 1)
				return Ok(result);

			return StatusCode(500, result);
		}



		[HttpPost]
		public async Task<IActionResult> RegisterStudent(User user)
		{
			var result = await _accountService.RegisterUser(user);

			if (result.Code == 1)
				return Ok(result);
			if (result.Code == -1)
				return Ok(result);

			return StatusCode(500, result);
		}
		[HttpPost]
		public async Task<IActionResult> RegisterProf(ProfInscriptionDTO user)
		{
			var result = await _accountService.RegisterTeacher(user);

			if (result.Code == 1)
				return Ok(result);
			if (result.Code == -1)
				return Ok(result);

			return StatusCode(500, result);
		}
		
		[HttpGet]
		public async Task<string> MailTest()
		{
			await _emailService.SendEmailAsync("tawba431@gmail.com","Test Email from Brevo",	
			"<h1>Hello!</h1><p>This is a <b>test email</b> sent via Brevo SMTP service.</p>");
			return "Git";
			
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
	}
}
