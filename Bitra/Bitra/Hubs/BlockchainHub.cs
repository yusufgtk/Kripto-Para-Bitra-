using Bitra.Services;
using Business.Abstracts;
using Entitiy.Entites;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace Bitra.Hubs
{
    public class BlockchainHub : Hub
    {
        private static ConcurrentDictionary<string, Node> _nodeList = new ConcurrentDictionary<string, Node>();

        public override async Task OnConnectedAsync()
        {
            string clientId = Context.ConnectionId;
            _nodeList.TryAdd(clientId, new Node
            {
                ConnectionId = clientId,
                IpAddress = Context.GetHttpContext()?.Connection.RemoteIpAddress?.ToString(),
                ConnectedAt = DateTime.UtcNow,
                LastActivity = DateTime.UtcNow,
            });

            await Clients.All.SendAsync("NewNodeConnected", clientId); //yeni düğümü herkes ile paylaş
            await Clients.Caller.SendAsync("NodeList", _nodeList.Keys); //yeni füğüme düğüm listesini paylaş
            await Clients.All.SendAsync("NodeCount", _nodeList.Count);

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            string clientId = Context.ConnectionId;
            _nodeList.TryRemove(clientId, out _);
            await Clients.All.SendAsync("NodeDisconnected", clientId);
            await Clients.All.SendAsync("NodeCount", _nodeList.Count);

            await base.OnDisconnectedAsync(exception);
        }

        // Genel mesaj gönderme
        public async Task BroadcastMessage(string message)
        {
            UpdateLastActivity(Context.ConnectionId);
            await Clients.All.SendAsync("ReceiveMessage", Context.ConnectionId, message);
        }

        // Özel mesaj gönderme
        public async Task SendDirectMessage(string targetId, string message)
        {
            UpdateLastActivity(Context.ConnectionId);
            await Clients.Client(targetId).SendAsync("ReceiveDirectMessage", Context.ConnectionId, message);
        }

        // Blockchain veri lari paylaş
        public async Task PublishChain(List<Block> chain)
        {
            UpdateLastActivity(Context.ConnectionId);
            await Clients.Others.SendAsync("ReceiveChain", Context.ConnectionId, chain);
        }

        // Bekleyen transactionları paylaş
        public async Task PendingTransaction(List<Transaction> pendingTransaction)
        {
            UpdateLastActivity(Context.ConnectionId);
            await Clients.Others.SendAsync("ReceivePendingTransaction", pendingTransaction);
        }

        // Düğüm aktivitesini güncelle
        private void UpdateLastActivity(string connectionId)
        {
            if (_nodeList.TryGetValue(connectionId, out var node))
            {
                node.LastActivity = DateTime.UtcNow;
            }
        }

        // Düğüm listesini almak için
        public async Task GetActiveNodes()
        {
            await Clients.Caller.SendAsync("NodeList", _nodeList.Keys);
        }

    }
}
