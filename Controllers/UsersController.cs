using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Services;
using DTO;
using Microsoft.AspNetCore.Authorization;

namespace Controllers
{
	[Authorize]
	[ApiController]
	[Route("api/[controller]/[action]")]
	public class UsersController : Controller
	{
		private readonly UserService _userService;

		public UsersController(UserService userService)
		{
			_userService = userService;
		}

		// GET api/users/5
		
	}
}
