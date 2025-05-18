using Application.Ports.Driven;
using DatabaseContext.Mappers;
using DatabaseContext.Models;
using Domain.Entities;
using Domain.Exceptions;
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
            var accountModel = AccountMapper.ToModel(account);

            _context
                .Accounts
                .Add(accountModel);
        }

        public async Task<Account?> GetAccountByDocumentAsync(string document)
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
                    .ThenInclude(asset => asset.Deposits)
                .Include(a => a.Orders)
                .FirstOrDefaultAsync(a => a.AccountId == accountId);
        }

        public async Task<Account?> GetAccountByAccountIdAsync(Guid accountId)
        {
            var accountModel = await GetAccountModelByAccountIdAsync(accountId);
            return accountModel is null ? null : AccountMapper.ToDomain(accountModel);
        }

        public async Task RegisterOrdersAsync(Account account)
        {
            AccountModel? accountModel;
            var trackedAccountModel = _context.ChangeTracker
                .Entries<AccountModel>()
                .FirstOrDefault(e => e.Entity.AccountId == account.AccountId);

            accountModel = trackedAccountModel is null
                ? await GetAccountModelByAccountIdAsync(account.AccountId)
                : trackedAccountModel.Entity;

            if (accountModel is null)
            {
                throw new EntityNotFoundException($"Account with ID {account.AccountId} not found.");
            }

            var newOrdersModel = account.Orders
                .Where(order => !accountModel.Orders.Any(orderModel => orderModel.OrderId == order.OrderId))
                .Select(order => OrderMapper.ToModel(order))
                .ToList();


            _context.Orders.AddRange(newOrdersModel);
        }
    }
}
