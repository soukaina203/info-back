using context;
using DTO;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Services
{
	public class ClassService : SuperService<Class>
	{
		private readonly AppDbContext _context;
		
		public ClassService (AppDbContext context) : base(context)
		{
			_context = context;
			
		}

		public async Task<ResponseDTO> GetClassesByProfId(int userId)
		{
			var classes = await _context.Classes.Where(c => c.UserId == userId).ToListAsync();
			if (classes!=null)
			{
				return new ResponseDTO { Code = 200, Message = "Success", Data = classes };
				
			}
				return new ResponseDTO { Code = 200, Message = "Success" };
 
		}
		
	}
}