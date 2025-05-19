using Application.DTOs;

namespace Application.Ports.Driving
{
    public interface ICredit
    {
        Task PlaceCreditAsync(TransactionDto transactionDto);
    }
}
