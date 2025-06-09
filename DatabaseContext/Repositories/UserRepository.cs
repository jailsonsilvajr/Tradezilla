using Application.Ports.Driven;
using DatabaseContext.Mappers;
using DatabaseContext.Models;
using Domain.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace DatabaseContext.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly TradezillaContext _context;

        public UserRepository(TradezillaContext context)
        {
            _context = context;
        }

        public void RegisterAccount(User account)
        {
            var accountModel = AccountMapper.ToModel(account);

            _context
                .Accounts
                .Add(accountModel);
        }

        public async Task<User?> GetUserByDocumentAsync(string document)
        {
            var accountModel = await _context
                .Accounts
                .FirstOrDefaultAsync(a => a.Document == document);

            return accountModel is null ? null : AccountMapper.ToDomain(accountModel);
        }

        public async Task<AccountModel?> GetAccountModelByAccountIdAsync(Guid accountId)
        {
            return await _context
                .Accounts
                .Include(a => a.Assets)
                    .ThenInclude(asset => asset.Transactions)
                .Include(a => a.Orders)
                .FirstOrDefaultAsync(a => a.AccountId == accountId);
        }

        public async Task<User?> GetUserByAccountIdAsync(Guid accountId)
        {
            var accountModel = await GetAccountModelByAccountIdAsync(accountId);
            return accountModel is null ? null : AccountMapper.ToDomain(accountModel);
        }
    }
}
