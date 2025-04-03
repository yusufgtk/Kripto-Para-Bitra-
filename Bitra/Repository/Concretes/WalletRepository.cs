
using Entitiy.Entites;
using Microsoft.EntityFrameworkCore;
using Repository.Abstracts;
using Repository.Contexts;

namespace Repository.Concretes
{
    public class WalletRepository : RepositoryBase<Wallet>, IWalletRepository
    {
        public WalletRepository(AppDbContext context) : base(context) { }


        public async Task<Wallet?> GetUserWalletByAddress(string address)
        {
            return await _context.Wallets.FirstOrDefaultAsync(w => w.Address == address && w.IsActive == true);
        }

        public async Task<Wallet?> GetServerWalletByAddress(string address)
        {
            return await _context.Wallets.FirstOrDefaultAsync(w => w.Address == address && w.Type == "server" && w.IsActive == true);
        }

        public async Task<decimal> GetTotalUserBalance()
        {
            return await _context.Wallets.Where(w => w.Type == "user").SumAsync(w => w.Balance);
        }

        public async Task<Wallet?> GetWalletByWalletAddressAndPrivateKey(string walletAddress, string privateKey)
        {
            return await _context.Wallets.FirstOrDefaultAsync(w => w.Address == walletAddress && w.PrivateKey == privateKey);
        }
    }
}
