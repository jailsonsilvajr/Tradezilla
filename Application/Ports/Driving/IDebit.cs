using Application.DTOs;

namespace Application.Ports.Driving
{
    public interface IDebit
    {
        Task PlaceDebitAsync(TransactionDto transactionDto);
    }
}
