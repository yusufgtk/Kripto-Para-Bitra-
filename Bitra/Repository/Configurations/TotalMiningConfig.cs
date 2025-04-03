

using Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Configurations
{
    class TotalMiningConfig : IEntityTypeConfiguration<TotalMining>
    {
        public void Configure(EntityTypeBuilder<TotalMining> builder)
        {
            builder.HasData(new TotalMining
            {
                Id = 1,
                CurrentTotal = 100,
                TotalPeek = 5000000,
                CreatedAt = DateTime.UtcNow,
                UpdateAt = DateTime.UtcNow,
            });
        }
    }
}
