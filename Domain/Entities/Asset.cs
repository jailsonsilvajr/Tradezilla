using Domain.Exceptions;
using Domain.Validators;

namespace Domain.Entities
{
    public class Asset
    {
        public static readonly int MAX_ASSETNAME_LENGTH = 5;
        private static readonly AssetValidator _validator = new AssetValidator();
        private readonly List<Deposit> _deposits = [];

        public Guid AssetId { get; set; }
        public Guid AccountId { get; set; }
        public string? AssetName { get; set; }
        public decimal Balance { get; private set; }
        public Account? Account { get; set; }
        public IReadOnlyCollection<Deposit> Deposits => _deposits.AsReadOnly();

        public Asset(Guid assetId, Guid accountId, string? assetName, List<Deposit> deposits)
        {
            AssetId = assetId;
            AccountId = accountId;
            AssetName = assetName;
            Balance = 0;

            foreach (var deposit in deposits)
            {
                AddDeposit(deposit);
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

        public void AddDeposit(Deposit deposit)
        {
            _deposits.Add(deposit);
            Balance += deposit.Quantity;
        }
    }
}
