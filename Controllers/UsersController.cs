using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Services;
using DTO;

namespace Controllers
{
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
