using Application.Ports.Driven;
using Domain.Entities;

namespace DatabaseContext.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly TradezillaContext _context;

        public AccountRepository(TradezillaContext context)
        {
            _context = context;
        }

        public Task RegisterAccountAsync(Account account)
        {
            throw new NotImplementedException();
        }
    }
}
