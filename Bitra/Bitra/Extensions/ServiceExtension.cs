using Business.Abstracts;
using Business.Concretes;
using Microsoft.EntityFrameworkCore;
using Repository.Abstracts;
using Repository.Concretes;
using Repository.Contexts;
using System.Reflection;

namespace Bitra.Extensions
{
    public static class ServiceExtension
    {
        public static void ConfigureAddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("SqlServer"), m => m.MigrationsAssembly("Bitra")));
        }

        public static void ConfigureAddRepository(this IServiceCollection services)
        {
            services.AddScoped<IBlockRepository, BlockRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<IWalletRepository, WalletRepository>();
            services.AddScoped<ITotalMiningRepository, TotalMiningRepository>();
        }

        public static void ConfigureAddBusiness(this IServiceCollection services)
        {
            services.AddScoped<IBlockchainService, BlockcahinManager>();
        }
    }
}
