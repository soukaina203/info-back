using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using context;
using System;

namespace Services
{
    public class SuperService<T> where T : class
    {
        protected readonly AppDbContext _context;

        public SuperService(AppDbContext context)
        {
            _context = context;
        }

        public virtual async Task<(List<T> list, int count)> GetAll()
        {
            var list = await _context.Set<T>().ToListAsync();
            int count = await _context.Set<T>().CountAsync();

            return (list, count);
        }

        public virtual async Task<T> GetById(int id)
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
                return new { m = "success" };
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return new { Message = ex.Message };
            }
        }

        public virtual async Task<object> Delete(int id)
        {
            var model = await _context.Set<T>().FindAsync(id);
            if (model != null)
            {
                _context.Set<T>().Remove(model);
                try
                {
                    await _context.SaveChangesAsync();
                    return true;
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    return new { Message = ex.Message };
                }
            }
            return false;
        }
    }
}
