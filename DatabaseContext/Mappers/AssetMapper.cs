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
                AssetId = asset.GetId(),
                AccountId = asset.GetAccountId(),
                AssetName = asset.GetAssetName(),
                Balance = asset.GetBalance()
            };
        }

        public static Asset ToDomain(AssetModel assetModel)
        {
            return new Asset(assetModel.AssetId, assetModel.AccountId, assetModel.AssetName);
        }
    }
}
