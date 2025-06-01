using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Services;
using Models;
using Microsoft.AspNetCore.Authorization;

namespace Controllers
{
	[Authorize]
	[ApiController]
	[Route("api/[controller]/[action]")]
	public class RoleController : SuperController<Role, RoleService> 
	{
			readonly RoleService _roleService;

		public RoleController(RoleService roleService) : base(roleService)
		{
			_roleService = roleService;
		}
	}
}