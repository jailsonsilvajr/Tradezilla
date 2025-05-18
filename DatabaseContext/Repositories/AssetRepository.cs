using Application.Ports.Driven;
using DatabaseContext.Mappers;
using Domain.Entities;

namespace DatabaseContext.Repositories
{
    public class AssetRepository : IAssetRepository
    {
        private readonly TradezillaContext _context;

        public AssetRepository(TradezillaContext context)
        {
            _context = context;
        }

        public void Insert(Asset asset)
        {
            var assetModel = AssetMapper.ToModel(asset);
            _context.Assets.Add(assetModel);
        }
    }
}
