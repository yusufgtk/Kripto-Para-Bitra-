

using Entitiy.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Configurations
{
    public class WalletConfigure : IEntityTypeConfiguration<Wallet>
    {
        public void Configure(EntityTypeBuilder<Wallet> builder)
        {
            builder.HasData(
                new Wallet
                {
                    Address = "BitraServerSecurityAddress",
                    PublicKey = Guid.NewGuid().ToString(),
                    PrivateKey = BCrypt.Net.BCrypt.HashPassword("yusufgtk"),
                    Balance = 100,
                    IsActive = true,
                    Type = "server"
                });
        }
    }
}
