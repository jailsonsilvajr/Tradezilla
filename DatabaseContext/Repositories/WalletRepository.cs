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
                ? throw new EntityNotFoundException($"Account {accountId} not found")
                : WalletMapper.ToDomain(accountModel);
        }

        private async Task<AccountModel> GetAccountTrackedAsync(Guid accountId)
        {
            AccountModel? accountModel;
            var trackedAccountModel = _context.ChangeTracker
                .Entries<AccountModel>()
                .FirstOrDefault(e => e.Entity.AccountId == accountId);

            accountModel = trackedAccountModel is null
                ? await GetAccountModelByAccountIdAsync(accountId)
                : trackedAccountModel.Entity;

            if (accountModel is null)
            {
                throw new EntityNotFoundException($"Account with ID {accountId} not found.");
            }

            return accountModel;
        }

        public async Task RegisterAssetAsync(Wallet wallet)
        {
            var accountModel = await GetAccountTrackedAsync(wallet.GetAccountId());

            var newAAssetModel = wallet.Assets
                .Where(asset => !accountModel.Assets.Any(assetModel => assetModel.AssetId == asset.GetId()))
                .Select(AssetMapper.ToModel)
                .ToList();

            var transactionsModel = accountModel.Assets.SelectMany(asset => asset.Transactions).ToList();

            var newTransactionsModel = wallet.Transactions
                .Where(t => !transactionsModel.Any(tm => tm.TransactionId == t.GetTransactionId()))
                .Select(TransactionMapper.ToModel)
                .ToList();

            _context.Assets.AddRange(newAAssetModel);
            _context.Transactions.AddRange(newTransactionsModel);
        }

        public async Task RegisterOrdersAsync(Wallet wallet)
        {
            var accountModel = await GetAccountTrackedAsync(wallet.GetAccountId());

            var newOrdersModel = wallet.Orders
                .Where(order => !accountModel.Orders.Any(orderModel => orderModel.OrderId == order.GetOrderId()))
                .Select(OrderMapper.ToModel)
                .ToList();


            _context.Orders.AddRange(newOrdersModel);
        }
    }
}
