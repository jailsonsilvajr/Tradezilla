using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;

namespace Domain.Aggregates
{
    public class Wallet
    {
        private readonly Account _account;
        private readonly List<Asset> _assets = [];
        private readonly List<Order> _orders = [];
        private readonly List<Transaction> _transactions = [];

        public IReadOnlyCollection<Asset> Assets => _assets.AsReadOnly();
        public IReadOnlyCollection<Order> Orders => _orders.AsReadOnly();
        public IReadOnlyCollection<Transaction> Transactions => _transactions.AsReadOnly();

        public Wallet(Guid accountId, List<Asset> assets, List<Order> orders, List<Transaction> transactions)
        {
            _account = new Account(accountId);

            foreach (var asset in assets)
            {
                AddAsset(asset);
            }

            foreach (var transaction in transactions)
            {
                AddTransaction(transaction);
            }

            foreach (var order in orders.OrderBy(o => o.GetCreatedDate()))
            {
                AddOrder(order);
            }
        }

        public Guid GetAccountId() => _account.GetId();

        public void AddAsset(Asset asset)
        {
            _assets.Add(asset);
        }

        public void AddOrder(Order order)
        {
            var assetName = order.GetSide()?.ToUpper() != "BUY"
                ? order.GetMarket()?.Split("/")[0].ToUpper()
                : order.GetMarket()?.Split("/")[1].ToUpper();

            var asset = Assets
                .FirstOrDefault(asset => asset.GetAssetName()?.ToUpper() == assetName);

            if (asset is null)
            {
                throw new EntityNotFoundException($"Asset {assetName} not found");
            }

            if (asset.GetBalance() < order.GetQuantity())
            {
                throw new InsufficientBalanceException($"Insufficient balance for asset {assetName}");
            }

            _orders.Add(order);
        }

        private void AddTransaction(Transaction transaction)
        {
            var asset = Assets.First(a => a.GetId() == transaction.GetAssetId());
            if (transaction.GetTransactionType() == TransactionType.Credit)
            {
                asset.AddCredit(transaction.GetQuantity());
            }
            else
            {
                asset.AddDebit(transaction.GetQuantity());
            }

            _transactions.Add(transaction);
        }

        public void AddTransaction(string assetName, int quantity, TransactionType transactionType)
        {
            if (transactionType == TransactionType.Credit)
            {
                AddCreditTransaction(assetName, quantity);
            }
            else
            {
                AddDebitTransaction(assetName, quantity);
            }
        }

        private void AddCreditTransaction(string assetName, int quantity)
        {
            var asset = Assets.FirstOrDefault(a => a.GetAssetName() == assetName);
            if (asset is null)
            {
                asset = Asset.Create(_account.GetId(), assetName);
                _assets.Add(asset);
            }

            _transactions.Add(new Transaction(Guid.NewGuid(), asset.GetId(), quantity, TransactionType.Credit));
            asset.AddCredit(quantity);
        }

        private void AddDebitTransaction(string assetName, int quantity)
        {
            var asset = Assets.FirstOrDefault(a => a.GetAssetName() == assetName)
                ?? throw new EntityNotFoundException($"Asset {assetName} not found");

            if (asset.GetBalance() < quantity)
            {
                throw new InsufficientBalanceException($"Insufficient balance to perform transaction");
            }

            _transactions.Add(new Transaction(Guid.NewGuid(), asset.GetId(), quantity, TransactionType.Debit));
            asset.AddDebit(quantity);
        }
    }
}
