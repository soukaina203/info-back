using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Services;
using Models;
namespace Controllers
{
	
	public class RoleController : SuperController<Role, RoleService> 
	{
			readonly RoleService _roleService;

		public RoleController(RoleService roleService) : base(roleService)
		{
			_roleService = roleService;
		}
	}
}