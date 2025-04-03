using Entitiy.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstracts
{
    public interface IBlockService
    {
        Task AddAsync(Block block);
        void Delete(Block block);
        void Update(Block block);
        Task<bool> SaveChangesAsync();

        string CalculateHash(Block block); //bloğu şifreleme
        void MineBlock(Block block, int difficulty); //bloğu kazma

        Task<IEnumerable<Block>> GetChainAsync();
        Task<Block?> GetLatestBlockAsync();
        Task<bool> IsChainValidAsync();

    }
}
