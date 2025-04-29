using Microsoft.EntityFrameworkCore;

namespace DatabaseContext
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
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
