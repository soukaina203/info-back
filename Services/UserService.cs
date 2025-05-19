using DTO;
using Models;
using Microsoft.EntityFrameworkCore;
using Services;
using context;
using DTO;
namespace Services
{
	public class UserService :SuperService<ProfInscriptionDTO>
	{
		private readonly AppDbContext _context;

		  public UserService(AppDbContext context) : base(context)  
    {
        _context = context;
    }

		// For teachers and students
		public override async Task<ProfInscriptionDTO> GetById(int id)
		{
			var user = await _context.Users.FindAsync(id);
			if (user == null)
			{
				throw new Exception($"User with ID {id} not found.");
			}

			if (user.RoleId == 1) // Prof
			{
				var prof = await _context.ProfProfiles
					.FirstOrDefaultAsync(p => p.UserId == id);

				if (prof == null)
				{
					throw new Exception($"Prof profile for user ID {id} not found.");
				}

			return new ProfInscriptionDTO {user = user ,profProfile=prof };
			}
			else
			{
			return new ProfInscriptionDTO {user = user ,profProfile=null };
			}
		}
	}
}
