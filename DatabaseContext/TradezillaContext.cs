using DatabaseContext.Config;
using DatabaseContext.Models;
using Microsoft.EntityFrameworkCore;

namespace DatabaseContext
{
    public class TradezillaContext : DbContext
    {
        public DbSet<AccountModel> Accounts { get; set; }
        public DbSet<AssetModel> Assets { get; set; }
        public DbSet<DepositModel> Deposits { get; set; }
        public DbSet<OrderModel> Orders { get; set; }

        public TradezillaContext(DbContextOptions<TradezillaContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new AccountConfig());
            modelBuilder.ApplyConfiguration(new AssetConfig());
            modelBuilder.ApplyConfiguration(new DepositConfig());
            modelBuilder.ApplyConfiguration(new OrderConfig());
        }
    }
}
