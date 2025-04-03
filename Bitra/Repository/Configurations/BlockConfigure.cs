

using Entitiy.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Security.Cryptography;
using System.Text;

namespace Repository.Configurations
{
    public class BlockConfigure : IEntityTypeConfiguration<Block>
    {
        public void Configure(EntityTypeBuilder<Block> builder)
        {
            Block genesisBlock = GenerateBlock(); 
            builder.HasData(genesisBlock);
        }

        public Block GenerateBlock()
        {
            Block block = new Block();
            block.Id = 1;
            block.Timestamp = DateTime.UtcNow;
            block.Transactions = new List<Transaction>();
            block.PreviousHash = "0";
            block.Nonce = 0;

            string transactions = string.Join("", block.Transactions.Select(t => t.Id));
            string blockData = block.Id.ToString() + block.Timestamp.ToString() + transactions + block.PreviousHash;

            StringBuilder stringBuilder = new StringBuilder();
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(blockData));
                foreach (byte b in bytes)
                {
                    stringBuilder.Append(b.ToString("x2"));
                }
            }

            block.Hash = stringBuilder.ToString();
            return block;
        }
    }
}
