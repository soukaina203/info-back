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
		
		public override async Task<object> GetAll()
		{
 		   var data = await _context.Classes.Include(c => c.User).ToListAsync();
			
			return new {Data = data};
			
		}
		
			public  async Task<object> SearchClasses(string date , string title ,string prof)
		{
				var query = _context.Classes.Include(r => r.User).AsQueryable();


			// Filter by title
			if (title != "null" && !string.IsNullOrEmpty(title))
			{
				query = query.Where(r => r.Title.ToLower().Contains(title.Trim().ToLower()));
			}

			// Filter by date (if provided)
			if (!string.IsNullOrEmpty(date))
			{
				if (DateTime.TryParse(date, out var parsedDate))
				{
					// Compare just the date parts (ignoring time)
					query = query.Where(r =>
						r.Date.Year == parsedDate.Year &&
						r.Date.Month == parsedDate.Month &&
						r.Date.Day == parsedDate.Day);
				}
			}

			// Filter by prof name
			if (prof != "null" && !string.IsNullOrEmpty(prof))
			{
				query = query.Where(r =>
					r.User.FirstName.Contains(prof) || r.User.LastName.Contains(prof)
				);
			}

			var list = await query.ToListAsync();
			return new
			{
				list = list,
				title = title,
				date = date,
				prof = prof,
			};
			
		}
		
		public override async Task<object> GetById(int id)
		{
			var data = await _context.Classes
			.Include(c => c.User)
			.FirstOrDefaultAsync(c => c.Id == id);			
				return new {Data = data};
			
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
			
		
		public async Task<object> SearchReunions(
			string? date,
			 string? title,
			 string? prof)
		{
			var query = _context.Classes.Include(r => r.User).AsQueryable();


			// Filter by title
			if (title != "null" && !string.IsNullOrEmpty(title))
			{
				query = query.Where(r => r.Title.ToLower().Contains(title.Trim().ToLower()));
			}

			// Filter by date (if provided)
			if (!string.IsNullOrEmpty(date))
			{
				if (DateTime.TryParse(date, out var parsedDate))
				{
					// Compare just the date parts (ignoring time)
					query = query.Where(r =>
						r.Date.Year == parsedDate.Year &&
						r.Date.Month == parsedDate.Month &&
						r.Date.Day == parsedDate.Day);
				}
			}

			// Filter by prof name
			if (prof != "null" && !string.IsNullOrEmpty(prof))
			{
				query = query.Where(r =>
					r.User.FirstName.Contains(prof) || r.User.LastName.Contains(prof)
				);
			}

			var list = await query.ToListAsync();
			return new
			{
				list = list,
				title = title,
				date = date,
				prof = prof,
			};
		}
		
		
		
		
	}
}