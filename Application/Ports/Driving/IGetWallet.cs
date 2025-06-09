using Application.DTOs;

namespace Application.Ports.Driving
{
    public interface IGetWallet
    {
        Task<WalletDto> GetWalletAsync(Guid accountId);
    }
}
