using Domain.Aggregates;

namespace Application.Ports.Driven
{
    public interface IWalletRepository
    {
        Task<Wallet> GetWalletByAccountIdAsync(Guid accountId);
        Task RegisterOrdersAsync(Wallet wallet);
        Task RegisterAssetAsync(Wallet wallet);
    }
}
