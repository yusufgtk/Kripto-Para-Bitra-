using Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Abstracts;
using Repository.Contexts;


namespace Repository.Concretes
{
    public class TotalMiningRepository : RepositoryBase<TotalMining>, ITotalMiningRepository
    {
        public TotalMiningRepository(AppDbContext context) : base(context) { }
        public async Task<TotalMining?> GetTotalMining()
        {
            return await _context.TotalMinings.FirstOrDefaultAsync(tm => tm.Id == 1);
        }
    }
}
