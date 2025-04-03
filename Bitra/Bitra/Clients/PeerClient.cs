

using Entity.Dtos;
using Microsoft.AspNetCore.SignalR.Client;

namespace Bitra.Clients

{
    public class PeerClient
    {
        private HubConnection connection;
        public static List<string> Peers { get; set; } = new List<string>();

        public PeerClient(string peerUrl)
        {
            connection = new HubConnectionBuilder().WithUrl(peerUrl).Build();

            connection.On<List<string>> ("UpdatePeerList", (peers) =>
            {
                Peers = peers;
                Console.WriteLine("Bağlı düğümler güncellendi:");
                foreach (var peer in Peers)
                {
                    Console.WriteLine(peer);
                }
            });

            connection.On<BlockDto>("ReceiveBlock", (block) =>
            {
                Console.WriteLine($"Yeni blok alındı: {block}");
            });
            
            connection.On<bool>("IsValidBlockChain", (isValid) =>
            {
                Console.WriteLine($"Blockcahin sorgulandı : {isValid}");
            });

            connection.On<TransactionDto>("NewTransactionAdded", (transaction) =>
            {
                Console.WriteLine($"Yeni bir işlem gerçekleşti => Id: {transaction.Id}, Sender:{transaction.Sender}, Receiver:{transaction.Receiver}, Amount:{transaction.Amount}, Timestamp:{transaction.Timestamp.ToString("dd/MM/yy - HH:mm:ss")}");
                Console.WriteLine($"{new string('#', 40)}");
            });
            
            connection.On<List<TransactionDto>>("PendingTransactions", (pendingTransactions) =>
            {
                Console.WriteLine("Pending Transactions");
                Console.WriteLine($"{new string('#', 40)}");
                foreach (var pt in pendingTransactions)
                {
                    Console.WriteLine($"Pending transaction => Id: {pt.Id}, Sender:{pt.Sender}, Receiver:{pt.Receiver}, Amount:{pt.Amount}, Timestamp:{pt.Timestamp.ToString("dd/MM/yy - HH:mm:ss")}");
                    Console.WriteLine($"{new string('#', 40)}");
                }
            });
            connection.On<decimal>("CurrentPrice", (currentPrice) =>
            {
                Console.WriteLine($"Price : {currentPrice}");
            });
            connection.On<BlockDto>("NewBlockAdded", (block) =>
            {
                Console.WriteLine($"Yeni bir blok kazıldı : {block}");
            });

            connection.On<string>("NewNodeConnected", (clientId) =>
            {
                Console.WriteLine($"Ağa yeni client katıldı: {clientId}");
            });

            connection.On<string>("NodeDisconnected", (clientId) =>
            {
                Console.WriteLine($"clien ağdan ayrıldı: {clientId}");
            });

        }

        public async Task Connect()
        {
            await connection.StartAsync();
            Console.WriteLine("Ağa bağlandım!");
        }

        public async Task SendBlock(string blockData)
        {
            await connection.InvokeAsync("BroadcastBlock", blockData);
        }
    }
}
