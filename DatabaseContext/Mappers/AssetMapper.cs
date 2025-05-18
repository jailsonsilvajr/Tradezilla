using DatabaseContext.Models;
using Domain.Entities;

namespace DatabaseContext.Mappers
{
    public static class AssetMapper
    {
        public static AssetModel ToModel(Asset asset)
        {
            return new AssetModel
            {
                AssetId = asset.AssetId,
                AccountId = asset.AccountId,
                AssetName = asset.AssetName,
                Balance = asset.Balance,
                Deposits = asset.Deposits.Select(d => DepositMapper.ToModel(d)).ToList()
            };
        }

        public static Asset ToDomain(AssetModel assetModel)
        {
            var deposits = assetModel.Deposits.Select(d => DepositMapper.ToDomain(d)).ToList();
            return new Asset(assetModel.AssetId, assetModel.AccountId, assetModel.AssetName, deposits);
        }
    }
}
