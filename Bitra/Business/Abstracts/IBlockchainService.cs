
using Entitiy.Entites;
using Repository.Responses;

namespace Business.Abstracts
{
    public interface IBlockchainService
    {
        Task<IEnumerable<Block>> GetChainAsync();
        Task<bool> IsChainValidAsync();
        List<Transaction> GetPendingTransactions();
        Task CreatedTransactionAsync(Transaction transaction);

        Task<Wallet> CreateWallet(string privateKey); //cüzdan oluştur
        Task<Wallet?> GetUserWalletByAddress(string address); //cüzdanı getir
        Task<Wallet?> GetServerWalletByAddress(string address); //cüzdanı getir
        Task<ApiResponse> ExecuteTransaction(string senderAddress, string receiverAddress, decimal amount, string signKey);

        Task<DataResponse<Wallet?>> GetWalletByWalletAddressAndPrivateKey(string walletAddress, string privateKey); //cüzdanı getir

        Task<decimal> CalculatePrice(); // fiyat hesapla
    }
}
