using Application.Ports.Driven;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

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
            _context
                .Accounts
                .Add(account);
        }

        public async Task<Account?> GetAccountByDocumentAsync(string document)
        {
            return await _context
                .Accounts
                .FirstOrDefaultAsync(a => a.Document == document);
        }

        public async Task<Account?> GetAccountByAccountIdAsync(Guid accountId)
        {
            return await _context
                .Accounts
                .Include(a => a.Assets)
                .Include(a => a.Orders)
                .FirstOrDefaultAsync(a => a.AccountId == accountId);
        }
    }
}
