using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Services;
using DTO;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        // GET api/users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProfInscriptionDTO>> GetById(int id)
        {
            try
            {
                var result = await _userService.GetById(id);
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                // Ici tu peux gérer les erreurs (NotFound, etc.)
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
