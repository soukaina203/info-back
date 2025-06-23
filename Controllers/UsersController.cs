using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Services;
using DTO;
using Microsoft.AspNetCore.Authorization;
using Models;
namespace Controllers
{
	[Authorize]
	[ApiController]
	[Route("api/[controller]/[action]")]
	public class UsersController : SuperController<User, UserService>  
	{
		private readonly UserService _userService;

		public UsersController(UserService userService) : base(userService)
		{
			_userService = userService;
		}

		[HttpGet("{id}")]
		public  async Task<IActionResult> GetUserById(int id)
		{
			var user=await	 _userService.GetUserById(id);
			return Ok(user);
			
		}
		
		[HttpPut("{id}")]
		public  async Task<IActionResult> PutUser(int id, ProfInscriptionDTO model)
		{
			var user=await	 _userService.PutUser(id,model);
			return Ok(user);
			
		}
		
		 [HttpGet("")]
		 public async Task<IActionResult> SearchUsers([FromQuery] string nom, [FromQuery] string prenom, [FromQuery] string email, [FromQuery] int roleId)
			{
				var result = await _userService.SearchUsers(nom, prenom, email, roleId);

				return Ok(new
				{
					query = new { result }
				});
			}
	}
}
