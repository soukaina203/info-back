using DTO;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace Controllers
{
	[ApiController]
	[Route("api/[controller]/[action]")]
	public class UploadController : ControllerBase
	{
		readonly UploadService _uploadService;

		public UploadController(UploadService uploadService)
		{
			_uploadService = uploadService;
		}

		[HttpPost("{folderName}")]
		public async Task<IActionResult> UploadFile(IFormFile file,string folderName)
		{
			var result = await _uploadService.UploadFile(file,folderName);

			if (result.Code == 1)
				return Ok(result);
			if (result.Code == -1)
				return Ok(result);

			return StatusCode(500, result);
		}



		[HttpGet("{folder}/{filename}")]
		public async Task<IActionResult> DownloadFile(string folder, string filename)
		{
			var fileDto = await _uploadService.DownloadFileAsync(folder, filename);

			if (fileDto == null)
			{
				return NotFound();
			}

			return File(fileDto.FileContent, fileDto.ContentType, fileDto.FileName);
		}




	}
}