using DTO;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace Controllers
{
	[ApiController]
	[Route("api/[controller]/[action]")]

	public class ClassController : ControllerBase
	{
		private readonly ClassService _classService;

		public ClassController(ClassService classService)
		{
			_classService = classService;
		}

		[HttpGet("{userId}")]
		public async Task<ActionResult> GetClassesByProfId(int userId)
		{
			try
			{
				var result = await _classService.GetClassesByProfId(userId);
				return Ok(result);
			}
			catch (System.Exception ex)
			{
				// Ici tu peux gérer les erreurs (NotFound, etc.)
				return Ok(new { message = ex.Message });
			}
		}
	}
}