using DatabaseContext.Config;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatabaseContext
{
    public class TradezillaContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }

        public TradezillaContext(DbContextOptions<TradezillaContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new AccountConfig());
        }
    }
}
