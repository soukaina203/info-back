using Microsoft.AspNetCore.Mvc;
using Models;           // Make sure you have a User or Account model in here
using context;          // This should be your actual namespace (e.g., MyApp.Data)
using Services;

namespace Controllers
{
	[ApiController]
	[Route("api/[controller]/[action]")]
	public class AccountController : ControllerBase
	{
		private readonly AppDbContext _context;
		private readonly AccountService _accountService;

		public AccountController(AppDbContext context , AccountService accountService )
		{
			_context = context;
			_accountService = accountService;
		}

		[HttpPost]
		public async Task<IActionResult> Register(User user)
		{
			var result = await _accountService.RegisterUser(user);

			if (result.Code == 1)
				return Ok(result);
			if (result.Code == -1)
				return Ok(result);

			return StatusCode(500, result);
		}
	}
}
