

using Entitiy.Entites;
using Entity.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Repository.Contexts
{
    public class AppDbContext : DbContext
    {
        public DbSet<Block> Blocks { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<TotalMining> TotalMinings { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //Genesis Block
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
