using Application.Ports.Driven;
using DatabaseContext.Mappers;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatabaseContext.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly TradezillaContext _context;

        public OrderRepository(TradezillaContext context)
        {
            _context = context;
        }

        public async Task<List<Order>?> GetOrdersByMarketIdAsync(string marketId)
        {
            var ordersModel = await _context.Orders
                .Where(o => o.Market == marketId)
                .ToListAsync();

            return ordersModel?.Select(o => OrderMapper.ToDomain(o)).ToList();
        }
    }
}
