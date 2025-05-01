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

        public async Task InsertAccountAsync(
            Guid id,
            string name,
            string email,
            string document,
            string password)
        {
            string sql = "INSERT INTO Accounts VALUES (@p0, @p1, @p2, @p3, @p4)";
            await Database.ExecuteSqlRawAsync(sql, id, name, email, document, password);
        }
    }
}
