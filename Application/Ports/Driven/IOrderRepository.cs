using Domain.Entities;

namespace Application.Ports.Driven
{
    public interface IOrderRepository
    {
        Task<List<Order>?> GetOrdersByMarketIdAsync(string marketId);
        Task<List<Order>?> GetOrdersOpensByMarketIdAsync(string marketId);
        void UpdateOrderExecuted(Order order);
    }
}
