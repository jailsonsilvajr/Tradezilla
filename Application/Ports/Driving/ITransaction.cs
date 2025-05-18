using Application.DTOs;

namespace Application.Ports.Driving
{
    public interface ITransaction
    {
        Task PlaceTransactionAsync(TransactionDto transactionDto);
    }
}
