using context;
using Models;
namespace Services
{
	public class RoleService : SuperService<Role>
	{
		private readonly AppDbContext _context;
		
		public RoleService (AppDbContext context) : base(context)
		{
			_context = context;
			
		}

	
		
	}
}