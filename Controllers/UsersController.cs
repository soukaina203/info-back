using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Services;
using DTO;
using Microsoft.AspNetCore.Authorization;
using Models;
namespace Controllers
{
	// [Authorize]
	[ApiController]
	[Route("api/[controller]/[action]")]
	public class UsersController : SuperController<User, UserService>  
	{
		private readonly UserService _userService;

		public UsersController(UserService userService) : base(userService)
		{
			_userService = userService;
		}

		// GET api/users/5
		[HttpGet("{id}")]
		
		public  async Task<IActionResult> GetUserById(int id)
		{
			var user=await	 _userService.GetUserById(id);
			return Ok(user);
			
		}
	}
}
