using Domain.Entities;
using Domain.Exceptions;
using Domain.ValueObjects;

namespace Domain.Aggregates
{
    public class Wallet
    {
        private readonly ID _accountId;
        private readonly List<Asset> _assets = [];
        private readonly List<Order> _orders = [];

        public IReadOnlyCollection<Asset> Assets => _assets.AsReadOnly();
        public IReadOnlyCollection<Order> Orders => _orders.AsReadOnly();

        public Wallet(Guid accountId, List<Asset> assets, List<Order> orders)
        {
            _accountId = new ID(accountId);

            foreach (var asset in assets)
            {
                AddAsset(asset);
            }

            foreach (var order in orders)
            {
                AddOrder(order);
            }
        }

        public Guid GetAccountId() => _accountId.GetValue();

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
