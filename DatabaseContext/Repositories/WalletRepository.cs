using Application.Ports.Driven;
using DatabaseContext.Mappers;
using DatabaseContext.Models;
using Domain.Aggregates;
using Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace DatabaseContext.Repositories
{
    public class WalletRepository : IWalletRepository
    {
        private readonly TradezillaContext _context;

        public WalletRepository(TradezillaContext context)
        {
            _context = context;
        }

        private async Task<AccountModel?> GetAccountModelByAccountIdAsync(Guid accountId)
        {
            return await _context
                .Accounts
                .Include(a => a.Assets)
                    .ThenInclude(asset => asset.Transactions)
                .Include(a => a.Orders)
                .FirstOrDefaultAsync(a => a.AccountId == accountId);
        }

        public async Task<Wallet> GetWalletByAccountIdAsync(Guid accountId)
        {
            var accountModel = await GetAccountModelByAccountIdAsync(accountId);

            return accountModel is null
                ? throw new EntityNotFoundException($"Wallet with AccountID {accountId} not found")
                : WalletMapper.ToDomain(accountModel);
        }

        public async Task RegisterOrdersAsync(Wallet wallet)
        {
            AccountModel? accountModel;
            var trackedAccountModel = _context.ChangeTracker
                .Entries<AccountModel>()
                .FirstOrDefault(e => e.Entity.AccountId == wallet.GetAccountId());

            accountModel = trackedAccountModel is null
                ? await GetAccountModelByAccountIdAsync(wallet.GetAccountId())
                : trackedAccountModel.Entity;

            if (accountModel is null)
            {
                throw new EntityNotFoundException($"Account with ID {wallet.GetAccountId()} not found.");
            }

            var newOrdersModel = wallet.Orders
                .Where(order => !accountModel.Orders.Any(orderModel => orderModel.OrderId == order.GetOrderId()))
                .Select(OrderMapper.ToModel)
                .ToList();


            _context.Orders.AddRange(newOrdersModel);
        }
    }
}
