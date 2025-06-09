using Application.DTOs;

namespace Application.Ports.Driving
{
    public interface IPlaceTransaction
    {
        Task PlaceTransactionAsync(TransactionDto transactionDto);
    }
}
