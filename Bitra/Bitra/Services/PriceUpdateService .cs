using Bitra.Hubs;
using Business.Abstracts;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Bitra.Services
{
    public class PriceUpdateService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IHubContext<BlockchainHub> _hubContext;
        private readonly TimeSpan _interval = TimeSpan.FromSeconds(5);
        public PriceUpdateService(IServiceProvider serviceProvider, IHubContext<BlockchainHub> hubContext)
        {
            _serviceProvider = serviceProvider;
            _hubContext = hubContext;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (true)
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var blockchainService = scope.ServiceProvider.GetRequiredService<IBlockchainService>();

                        try
                        {
                            decimal currentPrice = await blockchainService.CalculatePrice();
                            await _hubContext.Clients.All.SendAsync("CurrentPrice", currentPrice);
                            Console.WriteLine($"Toplam Piyasa Değeri: {currentPrice * 1000000000}");

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"[PriceUpdate] Hata oluştu: {ex.Message}");
                        }
                    }

                    await Task.Delay(_interval, stoppingToken);
                }
            }
        }
    }
}
