using Application.Ports.Driven;
using DatabaseContext.Mappers;
using DatabaseContext.Models;
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

        public async Task<List<Order>?> GetOrdersOpensByMarketIdAsync(string marketId)
        {
            var ordersModel = await _context.Orders
                .Where(o => o.Market == marketId && o.Status != null && o.Status.ToUpper() == "OPEN")
                .ToListAsync();

            return ordersModel?.Select(o => OrderMapper.ToDomain(o)).ToList();
        }

        public void UpdateOrderExecuted(Order order)
        {
            var trackedEntity = _context.ChangeTracker.Entries<OrderModel>()
                .FirstOrDefault(e => e.Entity.OrderId == order.GetOrderId());

            if (trackedEntity is not null)
            {
                trackedEntity.Entity.FillQuantity = order.FillQuantity;
                trackedEntity.Entity.FillPrice = order.FillPrice;
                trackedEntity.Entity.Status = order.Status;

                _context.Orders.Update(trackedEntity.Entity);
            }
            else
            {
                var orderModel = OrderMapper.ToModel(order);
                _context.Orders.Update(orderModel);
            }
        }
    }
}
