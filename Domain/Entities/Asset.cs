using Domain.Exceptions;
using Domain.Validators;

namespace Domain.Entities
{
    public class Asset
    {
        public static readonly int MAX_ASSETNAME_LENGTH = 5;
        private static readonly AssetValidator _validator = new AssetValidator();
        private readonly List<Transaction> _transactions = [];

        public Guid AssetId { get; set; }
        public Guid AccountId { get; set; }
        public string? AssetName { get; set; }
        public decimal Balance { get; private set; }
        public Account? Account { get; set; }
        public IReadOnlyCollection<Transaction> Transactions => _transactions.AsReadOnly();

        public Asset(Guid assetId, Guid accountId, string? assetName, List<Transaction> transactions)
        {
            AssetId = assetId;
            AccountId = accountId;
            AssetName = assetName;
            Balance = 0;

            foreach (var transation in transactions)
            {
                AddTransation(transation);
            }
        }

        public static Asset Create(Guid accountId, string? assetName)
        {
            var newAsset =  new Asset(Guid.NewGuid(), accountId, assetName, []);
            var validationResult = _validator.Validate(newAsset);
            if (!validationResult.IsValid)
            {
                throw new ValidationException("Invalid data to create asset", validationResult.Errors);
            }

            return newAsset;
        }

        public void AddTransation(Transaction transation)
        {
            _transactions.Add(transation);
            Balance += transation.Quantity;
        }
    }
}
