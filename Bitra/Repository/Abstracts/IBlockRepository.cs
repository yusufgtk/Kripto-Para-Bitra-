

using Entitiy.Entites;

namespace Repository.Abstracts
{
    public interface IBlockRepository : IRepositoryBase<Block>
    {
        Task<IEnumerable<Block>> GetChainAsync();
        Task<Block?> GetLatestBlockAsync();

        Task<bool> IsChainValidAsync();
    }
}
