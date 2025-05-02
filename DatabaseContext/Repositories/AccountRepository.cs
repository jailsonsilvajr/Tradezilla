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

        public async Task DeleteAccountByIdAsync(Guid accountId)
        {
            var account = await _context.Accounts.FindAsync(accountId);
            if (account != null)
            {
                _context.Accounts.Remove(account);
            }
        }
    }
}
