using Application.DTOs;

namespace Application.Ports.Driving
{
    public interface IDeposit
    {
        Task DepositAsync(DepositDto depositDto);
    }
}
