using Domain.Enums;
using Domain.Exceptions;
using Domain.Validators;
using Domain.ValueObjects;

namespace Domain.Entities
{
    public class Asset
    {
        public static readonly int MAX_ASSETNAME_LENGTH = 5;
        private static readonly AssetValidator _validator = new AssetValidator();
        private readonly List<Transaction> _transactions = [];

        private readonly ID _assetId;

        public Guid AccountId { get; set; }
        public string? AssetName { get; set; }
        public decimal Balance { get; private set; }
        public Account? Account { get; set; }
        public IReadOnlyCollection<Transaction> Transactions => _transactions.AsReadOnly();

        public Asset(Guid assetId, Guid accountId, string? assetName, List<Transaction> transactions)
        {
            _assetId = new ID(assetId);
            AccountId = accountId;
            AssetName = assetName;
            Balance = 0;

            foreach (var transation in transactions)
            {
                AddTransaction(transation);
            }

            Validate(this);
        }

        public Guid GetId() => _assetId.GetValue();

        public static Asset Create(Guid accountId, string? assetName)
        {
            var newAsset =  new Asset(Guid.NewGuid(), accountId, assetName, []);
            Validate(newAsset);
            return newAsset;
        }

        private static void Validate(Asset asset)
        {
            var validationResult = _validator.Validate(asset);
            if (!validationResult.IsValid)
            {
                throw new ValidationException("Invalid data to create asset", validationResult.Errors);
            }
        }

        public void AddTransaction(Transaction transaction)
        {
            if (transaction.TransactionType == TransactionType.Credit)
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
            Balance += transaction.Quantity;
        }

        private void AddDebitTransaction(Transaction transation)
        {
            if (Balance < transation.Quantity)
            {
                throw new InsufficientBalanceException($"Insufficient balance to perform transaction");
            }
            
            _transactions.Add(transation);
            Balance -= transation.Quantity;
        }
    }
}
