using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using context;
using System;
using Interfaces; 
using DTO; 
namespace Services
{
	public class SuperService<T> :  IService<T> where T : class
	{
		protected readonly AppDbContext _context;

		public SuperService(AppDbContext context)
		{
			_context = context;
		}

		public virtual async Task<object> GetAll()
		{
			var data = await _context.Set<T>().ToListAsync();
			
			return new {Data = data};
		}

		public virtual async Task<object> GetById(int id)
		{
			return await _context.Set<T>().FindAsync(id);
		}

		public virtual async Task<object> Post(T model)
		{
			await _context.Set<T>().AddAsync(model);

			try
			{
				await _context.SaveChangesAsync();
				return new { Message = "success" };
			}
			catch (DbUpdateConcurrencyException ex)
			{
				return new { Message = ex.Message };
			}
		}

		public virtual async Task<object> Put(int id, T model)
		{
			_context.Entry(model).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
				return new { Message = "success" };
			}
			catch (DbUpdateConcurrencyException ex)
			{
				return new { Message = ex.Message };
			}
		}

		public virtual async Task<ResponseDTO> Delete(int id)
		{
			var model = await _context.Set<T>().FindAsync(id);
			if (model != null)
			{
				_context.Set<T>().Remove(model);
				try
				{
					await _context.SaveChangesAsync();
					return new ResponseDTO { Code = 200, Message = "Deleted successfully" };
				}
				catch (DbUpdateConcurrencyException ex)
				{
					return new ResponseDTO { Code = 409, Message = ex.Message };
				}
			}
			return new ResponseDTO { Code = 404, Message = "Item not found" };
		}

	}
}
