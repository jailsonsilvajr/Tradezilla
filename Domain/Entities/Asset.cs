using Domain.Enums;
using Domain.Exceptions;
using Domain.ValueObjects;

namespace Domain.Entities
{
    public class Asset
    {
        private readonly List<Transaction> _transactions = [];

        private readonly ID _assetId;
        private readonly ID _accountId;
        private readonly AssetName _assetName;
        private Balance _balance;

        public Account? Account { get; set; }
        public IReadOnlyCollection<Transaction> Transactions => _transactions.AsReadOnly();

        public Asset(Guid assetId, Guid accountId, string assetName, List<Transaction> transactions)
        {
            _assetId = new ID(assetId);
            _accountId = new ID(accountId);
            _assetName = new AssetName(assetName);
            _balance = new Balance(0);

            foreach (var transation in transactions)
            {
                AddTransaction(transation);
            }
        }

        public Guid GetId() => _assetId.GetValue();
        public Guid GetAccountId() => _accountId.GetValue();
        public string GetAssetName() => _assetName.GetValue();
        public decimal GetBalance() => _balance.GetValue();

        public static Asset Create(Guid accountId, string assetName)
        {
            var newAsset =  new Asset(Guid.NewGuid(), accountId, assetName, []);
            return newAsset;
        }

        public void AddTransaction(Transaction transaction)
        {
            if (transaction.GetTransactionType() == TransactionType.Credit)
            {
                AddCreditTransaction(transaction);
            }
            else
            {
                AddDebitTransaction(transaction);
            }
        }

        private void AddCreditTransaction(Transaction transaction)
        {
            _transactions.Add(transaction);
            _balance = new Balance(_balance.GetValue() + transaction.GetQuantity());
        }

        private void AddDebitTransaction(Transaction transaction)
        {
            if (GetBalance() < transaction.GetQuantity())
            {
                throw new InsufficientBalanceException($"Insufficient balance to perform transaction");
            }
            
            _transactions.Add(transaction);
            _balance = new Balance(_balance.GetValue() - transaction.GetQuantity());
        }
    }
}
