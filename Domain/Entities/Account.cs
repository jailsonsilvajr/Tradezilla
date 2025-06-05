using Domain.Exceptions;
using Domain.ValueObjects;

namespace Domain.Entities
{
    public class Account
    {
        private readonly List<Asset> _assets = [];
        private readonly List<Order> _orders = [];

        private readonly ID _id;
        private readonly Name _name;
        private readonly Email _email;
        private readonly Document _document;
        private readonly Password _password;
        public IReadOnlyCollection<Asset> Assets => _assets.AsReadOnly();
        public IReadOnlyCollection<Order> Orders => _orders.AsReadOnly();

        public Account(Guid accountId, string? name, string? email, string document, string password, List<Asset> assets, List<Order> orders)
        {
            _id = new ID(accountId);
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
        }

        public Guid GetId() => _id.GetValue();
        public string GetName() => _name.GetValue();
        public string GetEmail() => _email.GetValue();
        public string GetDocument() => _document.GetValue();
        public string GetPassword() => _password.GetValue();

        public static Account Create(string? name, string? email, string document, string password)
        {
            var newAccount = new Account(Guid.NewGuid(), name, email, document, password, [], []);
            return newAccount;
        }

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
    }
}
