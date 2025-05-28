using Domain.Exceptions;
using Domain.Validators;
using Domain.ValueObjects;

namespace Domain.Entities
{
    public class Account
    {
        private static readonly AccountValidator _validator = new AccountValidator();
        private readonly List<Asset> _assets = [];
        private readonly List<Order> _orders = [];

        private readonly Name _name;
        private readonly Email _email;
        private readonly Document _document;
        private readonly Password _password;

        public Guid AccountId { get; }
        public IReadOnlyCollection<Asset> Assets => _assets.AsReadOnly();
        public IReadOnlyCollection<Order> Orders => _orders.AsReadOnly();

        public Account(Guid accountId, string? name, string? email, string document, string password, List<Asset> assets, List<Order> orders)
        {
            AccountId = accountId;
            _name = new Name(name);
            _email = new Email(email);
            _document = new Document(document);
            _password = new Password(password);

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

        public string GetName() => _name.GetValue();
        public string GetEmail() => _email.GetValue();
        public string GetDocument() => _document.GetValue();
        public string GetPassword() => _password.GetValue();

        public static Account Create(string? name, string? email, string document, string password)
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
