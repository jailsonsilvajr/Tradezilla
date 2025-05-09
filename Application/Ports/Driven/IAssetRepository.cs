using Domain.Entities;

namespace Application.Ports.Driven
{
    public interface IAssetRepository
    {
        void Insert(Asset asset);
    }
}
