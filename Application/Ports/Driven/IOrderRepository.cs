using Domain.Entities;

namespace Application.Ports.Driven
{
    public interface IOrderRepository
    {
        Task<List<Order>?> GetOrdersByMarketIdAsync(string marketId);
    }
}
