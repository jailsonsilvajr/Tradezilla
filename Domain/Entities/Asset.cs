using Domain.Exceptions;
using Domain.ValueObjects;

namespace Domain.Entities
{
    public class Asset
    {
        private readonly ID _assetId;
        private readonly ID _accountId;
        private readonly AssetName _assetName;
        private Balance _balance;

        public Account? Account { get; set; }

        public Asset(Guid assetId, Guid accountId, string assetName)
        {
            _assetId = new ID(assetId);
            _accountId = new ID(accountId);
            _assetName = new AssetName(assetName);
            _balance = new Balance(0);
        }

        public Guid GetId() => _assetId.GetValue();
        public Guid GetAccountId() => _accountId.GetValue();
        public string GetAssetName() => _assetName.GetValue();
        public decimal GetBalance() => _balance.GetValue();

        public static Asset Create(Guid accountId, string assetName)
        {
            var newAsset =  new Asset(Guid.NewGuid(), accountId, assetName);
            return newAsset;
        }

        public void AddCredit(int quantity)
        {
            _balance = new Balance(_balance.GetValue() + quantity);
        }

        public void AddDebit(int quantity)
        {
            if (GetBalance() < quantity)
            {
                throw new InsufficientBalanceException($"Insufficient balance to perform transaction");
            }

            _balance = new Balance(_balance.GetValue() - quantity);
        }
    }
}
