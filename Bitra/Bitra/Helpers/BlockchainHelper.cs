

using Business.Abstracts;
using Entitiy.Entites;

namespace Bitra.Services
{
    public static class BlockchainHelper
    {
        public static List<Transaction> PendingTransactions { get; set; } = new List<Transaction>();
        //public static int Difficulty { get; set; } = 2;
        //public static decimal MiningReward { get; set; } = 5;
        private static readonly object _lock = new object();

        public static List<Transaction> GetPendingTransactions()
        {
            return PendingTransactions;
        }


        public static void CreatedTransaction(Transaction transaction)
        {
            lock (_lock)
            {
                PendingTransactions.Add(transaction);
            }
        }


        //public async Task<Block> MinePendingTransactionAsync(string miningRewardAddress)
        //{
        //    List<Transaction> transactionsToMine;

        //    lock(_lock)
        //    {
        //        //bekleyen işlemlerin kopyasını al
        //        transactionsToMine = new List<Transaction>(PendingTransactions);

        //        //ödül işlemi oluştur - madenciye verilen ödül
        //        transactionsToMine.Add(new Transaction(null, miningRewardAddress, MiningReward));
        //    }

        //    Block previousBlock = GetLatestBlock();

        //    Block newBlock = new Block(previousBlock.Index + 1, transactionsToMine, previousBlock.Hash);

        //    // Async olarak mine et
        //    await Task.Run(() => newBlock.MineBlock(Difficulty));

        //    lock (_lock)
        //    {
        //        Chain.Add(newBlock); // zincire ekle

        //        //işlenen işlemleri bekleyen işlemlerden sil
        //        foreach(var tx in transactionsToMine)
        //        {
        //            if (PendingTransactions.Contains(tx))
        //            {
        //                PendingTransactions.Remove(tx);
        //            }
        //        }
        //    }
        //    return newBlock;
        //}

        //public decimal GetBalanceForAddrees(string address)
        //{
        //    decimal balance = 0;
        //    foreach(Block block in Chain)
        //    {
        //        foreach (Transaction transaction in block.Transactions)
        //        {
        //            if(transaction.Sender == address)
        //            {
        //                balance -= transaction.Amount;
        //            }

        //            if(transaction.Receiver == address)
        //            {
        //                balance += transaction.Amount;
        //            }
        //        }
        //    }
        //    return balance;
        //}

    }
   
}
