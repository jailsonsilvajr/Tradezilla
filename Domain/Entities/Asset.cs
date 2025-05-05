using Domain.Exceptions;
using Domain.Validators;

namespace Domain.Entities
{
    public class Asset
    {
        public static readonly int MAX_ASSETNAME_LENGTH = 5;
        private static readonly AssetValidator _validator = new AssetValidator();

        public Guid AssetId { get; set; }
        public string? AssetName { get; set; }
        public decimal Balance { get; set; }
        public ICollection<Deposit> Deposits { get; set; }

        private Asset(Guid assetId, string? assetName, decimal balance)
        {
            AssetId = assetId;
            AssetName = assetName;
            Balance = balance;
            Deposits = new List<Deposit>();
        }

        public static Asset Create(Guid assetId, string? assetName, decimal balance)
        {
            var newAsset =  new Asset(assetId, assetName, balance);
            var validationResult = _validator.Validate(newAsset);
            if (!validationResult.IsValid)
            {
                throw new ValidationException("Invalid data to create asset", validationResult.Errors);
            }

            return newAsset;
        }
    }
}
