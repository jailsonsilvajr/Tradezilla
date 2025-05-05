using Application.DTOs;

namespace Application.Ports.Driving
{
    public interface IGetAccount
    {
        Task<AccountDto> GetAccountByAccountIdAsync(Guid accountId);
    }
}
