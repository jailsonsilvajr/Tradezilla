using Domain.Exceptions;
using Domain.Validators;

namespace Domain.Entities
{
    public class Account
    {
        public static readonly int MAX_NAME_LENGTH = 100;
        public static readonly int MAX_EMAIL_LENGTH = 50;
        public static readonly int MAX_DOCUMENT_LENGTH = 11;
        public static readonly int MAX_PASSWORD_LENGTH = 14;
        private static readonly AccountValidator _validator = new AccountValidator();
        private readonly List<Asset> _assets = [];
        private readonly List<Order> _orders = [];

        public Guid AccountId { get; }
        public string? Name { get; }
        public string? Email { get; }
        public string? Document { get; }
        public string? Password { get; }
        public IReadOnlyCollection<Asset> Assets => _assets.AsReadOnly();
        public IReadOnlyCollection<Order> Orders => _orders.AsReadOnly();

        public Account(Guid accountId, string? name, string? email, string? document, string? password, List<Asset> assets, List<Order> orders)
        {
            AccountId = accountId;
            Name = name;
            Email = email;
            Document = CleanDocument(document);
            Password = password;

            foreach (var asset in assets)
            {
                AddAsset(asset);
            }

            foreach (var order in orders)
            {
                AddOrder(order);
            }

            Validate(this);
        }

        public static Account Create(string? name, string? email, string? document, string? password)
        {
            var newAccount = new Account(Guid.NewGuid(), name, email, document, password, [], []);
            Validate(newAccount);
            return newAccount;
        }

        private static void Validate(Account account)
        {
            var validationResult = _validator.Validate(account);
            if (!validationResult.IsValid)
            {
                throw new ValidationException("Invalid data to create account", validationResult.Errors);
            }
        }

        public static string? CleanDocument(string? document)
        {
            return document is null 
                ? default 
                : document.Trim().Replace(".", "").Replace("-", "");
        }

        public void AddAsset(Asset asset)
        {
            _assets.Add(asset);
        }

        public void AddOrder(Order order)
        {
            var assetName = order.Side?.ToUpper() != "BUY"
                ? order.Market?.Split("/")[0].ToUpper()
                : order.Market?.Split("/")[1].ToUpper();

            var asset = Assets
                .FirstOrDefault(asset => asset.AssetName?.ToUpper() == assetName);

            if (asset is null)
            {
                throw new EntityNotFoundException($"Asset {assetName} not found");
            }

            if (asset.Balance < order.Quantity)
            {
                throw new InsufficientBalanceException($"Insufficient balance for asset {assetName}");
            }

            _orders.Add(order);
        }
    }
}
