using Bitra.Clients;

namespace Bitra.Services
{
    public class PeerClientService : BackgroundService
    {
        private readonly PeerClient peerClient;

        public PeerClientService()
        {
            string peerUrl = "https://localhost:7012/blockchainhub";
            peerClient = new PeerClient(peerUrl);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await peerClient.Connect();
        }
    }
}
