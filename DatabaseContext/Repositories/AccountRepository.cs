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

        public void RegisterAccount(Account account)
        {
            _context.Accounts.Add(account);
        }
    }
}
