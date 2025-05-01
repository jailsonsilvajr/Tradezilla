using Application.Ports.Driven;

namespace DatabaseContext.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TradezillaContext _context;

        public UnitOfWork(TradezillaContext context)
        {
            _context = context;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
