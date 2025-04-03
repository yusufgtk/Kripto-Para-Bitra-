

using Entity.Entities;

namespace Repository.Abstracts
{
    public interface ITotalMiningRepository : IRepositoryBase<TotalMining>
    {
        Task<TotalMining?> GetTotalMining();
    }
}
