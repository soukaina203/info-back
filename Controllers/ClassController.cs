using DTO;
using Microsoft.AspNetCore.Mvc;
using Services;
using Models;
using Microsoft.AspNetCore.Authorization;

namespace Controllers
{
	// [Authorize]
	[ApiController]
	[Route("api/[controller]/[action]")]

	public class ClassController : SuperController<Class, ClassService> 
	{
		private readonly ClassService _classService;

		public ClassController(ClassService classService) : base(classService)
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
		
		
		
		
		[HttpGet("")]
		[AllowAnonymous]
		public async Task<ActionResult> GetClassesByProfId(	
			[FromQuery] string? date,
			[FromQuery] string? title,
			[FromQuery] string? prof)
		{
			try
			{
				var result = await _classService.SearchReunions(date , title , prof);
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