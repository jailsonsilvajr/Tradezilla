using Domain.Entities;

namespace Application
{
    public interface IBook
    {
        void AddOrder(Order order);
        Task MatchOrderAsync(string marketId);
    }
}
