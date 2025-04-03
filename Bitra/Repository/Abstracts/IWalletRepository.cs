

using Entitiy.Entites;

namespace Repository.Abstracts
{
    public interface IWalletRepository : IRepositoryBase<Wallet>
    {
        Task<Wallet?> GetUserWalletByAddress(string address);
        Task<Wallet?> GetServerWalletByAddress(string address);

        Task<Wallet?> GetWalletByWalletAddressAndPrivateKey(string walletAddress, string privateKey);

        Task<decimal> GetTotalUserBalance();

    }
}
