using Application.Ports.Driven;
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
            _context.Assets.Add(asset);
        }
    }
}
