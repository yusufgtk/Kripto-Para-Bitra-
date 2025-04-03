

using Entitiy.Entites;
using Microsoft.EntityFrameworkCore;
using Repository.Abstracts;
using Repository.Contexts;

namespace Repository.Concretes
{
    public class RepositoryBase<T> : IRepositoryBase<T>
    where T : class
    {
        protected readonly AppDbContext _context;
        public RepositoryBase(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }

        public async Task<bool> SaveChangesAsync()
        {

            try
            {
                return await _context.SaveChangesAsync() > 0; 
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
