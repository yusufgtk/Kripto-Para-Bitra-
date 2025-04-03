
using Business.Abstracts;
using Entitiy.Entites;
using Entity.Entities;
using Repository.Abstracts;
using Repository.Responses;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;



namespace Business.Concretes
{
    public class BlockcahinManager : IBlockchainService
    {
        public static List<Transaction> PendingTransactions { get; set; } = new List<Transaction>();
        public const string _serverAddress = "BitraServerSecurityAddress";
        public static int Difficulty { get; set; } = 2;
        private static readonly object _lock = new object();
        private const decimal initialReward = 100;
        private const int blockInterval = 100000;


        private readonly IBlockRepository _blockRepository;
        private readonly IWalletRepository _walletRepository;
        private readonly ITotalMiningRepository _totalMiningRepository;
        public BlockcahinManager(IBlockRepository blockRepository, IWalletRepository walletRepository, ITotalMiningRepository totalMiningRepository)
        {
            _blockRepository = blockRepository;
            _walletRepository = walletRepository;
            _totalMiningRepository = totalMiningRepository;
        }



        public async Task<IEnumerable<Block>> GetChainAsync()
        {
            return await _blockRepository.GetChainAsync();
        }

        public async Task<bool> IsChainValidAsync()
        {
            return await _blockRepository.IsChainValidAsync();
        }


        public List<Transaction> GetPendingTransactions()
        {
            return PendingTransactions;
        }

        public async Task<Wallet?> CreateWallet(string privateKey)
        {
            // Generate a new key pair
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                try
                {
                    string publicKey = Convert.ToBase64String(rsa.ExportRSAPublicKey());

                    if ((publicKey is null) || (privateKey is null))
                        return null;


                    Wallet newWallet = new Wallet
                    {
                        PublicKey = publicKey,
                        PrivateKey = BCrypt.Net.BCrypt.HashPassword(privateKey),
                        Address = GenerateWalletAddress(publicKey),
                        Balance = 0,
                        IsActive = true,
                        Type = "user"
                    };

                    await _walletRepository.AddAsync(newWallet);

                    if (!await _walletRepository.SaveChangesAsync())
                        return null;

                    return newWallet;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return null;
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }
            }
        }

        public async Task<Wallet?> GetUserWalletByAddress(string address) => await _walletRepository.GetUserWalletByAddress(address);
        public async Task<Wallet?> GetServerWalletByAddress(string address) => await _walletRepository.GetServerWalletByAddress(_serverAddress);
        public async Task<ApiResponse> ExecuteTransaction(string senderAddress, string receiverAddress, decimal amount, string signKey)
        {
            Wallet? senderWallet = await _walletRepository.GetUserWalletByAddress(senderAddress);
            if (!BCrypt.Net.BCrypt.Verify(signKey, senderWallet.PrivateKey)) // ikisi aynımı kontrol et
                return new ApiResponse(400, "BadRequest", "SignKey is invalid!");

            Wallet? receiverWallet = await _walletRepository.GetUserWalletByAddress(receiverAddress);

            if (senderWallet == null || receiverWallet == null) return new ApiResponse(404, "NotFound!", "Wallet is not found!");

            if(senderWallet.Balance > 0 && senderWallet.Balance >= amount)
            {
                senderWallet.Balance -= amount;
                receiverWallet.Balance += amount;
                if (await _walletRepository.SaveChangesAsync()) return new ApiResponse(200, "Success!", "Transaction is complated!");
            }

            return new ApiResponse(400, "BadRequest!", "You insufficient balance!");
        }



        public async Task CreatedTransactionAsync(Transaction transaction)
        {
            bool shouldMine = false;
            lock (_lock)
            {
                transaction.Id = Guid.NewGuid();
                transaction.Hash = CalculateHashTransaction(transaction);
                PendingTransactions.Add(transaction);
                shouldMine = PendingTransactions.Count >= 4;
            }

            if (shouldMine)
            {
                try
                {
                    Block? newBlock = await MinePendingTransactionAsync(_serverAddress);
                    if(newBlock != null)
                    {
                        await UpdateMinerBalanceAsync(newBlock);
                        Console.WriteLine("Yeni Blok Kazıldı!");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Blok kazılırken hata oluştu: {ex.Message}");
                }
            }
        }



        public async Task<decimal> CalculatePrice()
        {
            Wallet? serverWallet = await _walletRepository.GetServerWalletByAddress(_serverAddress);
            TotalMining? totalMining = await _totalMiningRepository.GetTotalMining();
            if (serverWallet is null || totalMining is null) return 0;

            decimal totalUserBalance = await _walletRepository.GetTotalUserBalance();
            totalUserBalance = totalUserBalance == 0 ? 1 : totalUserBalance;
            decimal price = (totalMining.CurrentTotal / totalMining.TotalPeek) * (totalUserBalance / (totalMining.CurrentTotal == 0 ? 1 : (totalMining.CurrentTotal))) * 1000;
            return price;
        }

        //Cüzdan için yetki alır
        public async Task<DataResponse<Wallet?>> GetWalletByWalletAddressAndPrivateKey(string walletAddress, string privateKey)
        {
            Wallet? wallet = await _walletRepository.GetUserWalletByAddress(walletAddress);
            if (wallet is null)
                return new DataResponse<Wallet?>(404, "Not Found", "Wallet is not found!", null);

            bool check = BCrypt.Net.BCrypt.Verify(privateKey, wallet?.PrivateKey);
            if (check)
            {
                return new DataResponse<Wallet?>(200, "Success", "Get wallet success.", wallet);
            }
            return new DataResponse<Wallet?>(400, "Bad Request", "Wallet private key is bad request!", null); ;
        }


        //////////////////////////// helper private methods //////////////////////////
        public async Task<Block?> MinePendingTransactionAsync(string miningRewardAddress)
        {
            List<Transaction> transactionsToMine;
            lock (_lock)
            {
                transactionsToMine = new List<Transaction>(PendingTransactions);//bekleyen işlemlerin kopyasını al
            }

            Block? previousBlock = await _blockRepository.GetLatestBlockAsync();
            if (previousBlock is null) return null;

            Block newBlock = new Block
            {
                Id = previousBlock.Id + 1,
                Timestamp = DateTime.UtcNow,
                PreviousHash = previousBlock.Hash,
                Nonce = 0,
                Transactions = transactionsToMine,
            };
            
            // Add the reward transaction
            Transaction rewardTransaction = new Transaction
            {
                Id = Guid.NewGuid(),
                Sender = null,
                Receiver = miningRewardAddress,
                Amount = await CalculateReward(initialReward, newBlock.Id, blockInterval),
                Timestamp = DateTime.UtcNow
            };
            rewardTransaction.Hash = CalculateHashTransaction(rewardTransaction);
            newBlock.Transactions.Add(rewardTransaction); // bekleyen transactionları bloğa yerleştir

            
            newBlock.Hash = CalculateHashBlock(newBlock); // bloğu şifrele

            await Task.Run(() => MineBlock(newBlock, Difficulty)); // bloğu kaz

            await _blockRepository.AddAsync(newBlock); // zincire ekle (save)
            if (!await _blockRepository.SaveChangesAsync()) return null;

            lock (_lock)
            {
                //işlenen işlemleri bekleyen işlemlerden sil
                foreach (var tx in transactionsToMine)
                {
                    if (PendingTransactions.Contains(tx))
                    {
                        PendingTransactions.Remove(tx);
                    }
                }
            }
            return newBlock;
        }

        //transaction şifreleme
        private string CalculateHashTransaction(Transaction transaction)
        {
            string transactionData = transaction.Id.ToString() + transaction.Sender + transaction.Receiver + transaction.Amount.ToString() + transaction.SignKey + transaction.Timestamp.ToString();
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(transactionData));
                StringBuilder stringBuilder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    stringBuilder.Append(b.ToString("x2"));
                }
                return stringBuilder.ToString();
            }
        }

        //Blok şifreleme
        private string CalculateHashBlock(Block block)
        {
            string blockData = block.Id.ToString() + block.Timestamp.ToString() + block.PreviousHash + block.Nonce.ToString();
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(blockData));
                StringBuilder stringBuilder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    stringBuilder.Append(b.ToString("x2"));
                }
                return stringBuilder.ToString();
            }
        }


        //kazma işlemi
        public void MineBlock(Block block, int difficulty)
        {
            string target = new string('0', difficulty);
            while (block.Hash.Substring(0, difficulty) != target)
            {
                block.Nonce++;
                block.Hash = CalculateHashBlock(block);
            }
            Console.WriteLine("Block kazıldı:" + block.Hash);
            
        }

        //Cüzdan adresi üretme
        private string GenerateWalletAddress(string publicKey)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(publicKey));
                return BitConverter.ToString(hashBytes).Replace("-", "").Substring(0, 40); // İlk 40 karakteri al
            }
        }

        //Server bakiyeyi günceller
        private async Task UpdateMinerBalanceAsync(Block block)
        {
            decimal reward = await CalculateReward(initialReward, block.Id, blockInterval);
            Wallet? wallet = await _walletRepository.GetUserWalletByAddress(_serverAddress);
            TotalMining? totalMinig = await _totalMiningRepository.GetTotalMining();
            if(totalMinig is not null && wallet is not null)
            {
                wallet.Balance += reward;
                totalMinig.CurrentTotal += reward;
                await _walletRepository.SaveChangesAsync();
            }
            
        }

        //ödül miktarını hesaplama
        private async Task<decimal> CalculateReward(decimal initialReward, int blockIndex, int blockInterval)
        {
            TotalMining? totalMining = await _totalMiningRepository.GetTotalMining();
            if (totalMining is null || totalMining.CurrentTotal >= totalMining.TotalPeek) return 0;

            //ödülün yarıya inmesi
            int halvingCount = (int)Math.Floor((decimal)blockIndex / blockInterval);
            decimal reward = initialReward * (decimal)Math.Pow(0.5, halvingCount);
            return reward;
        }

        
    }
}
