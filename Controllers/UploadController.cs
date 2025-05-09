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

        [HttpPost]

        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            var result = await _uploadService.UploadFile(file);

            if (result.Code == 1)
                return Ok(result);
            if (result.Code == -1)
                return Ok(result);

            return StatusCode(500, result);
        }




    }
}