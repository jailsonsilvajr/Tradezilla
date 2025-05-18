using Application.Ports.Driven;
using DatabaseContext.Mappers;
using Domain.Entities;

namespace DatabaseContext.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly TradezillaContext _context;

        public OrderRepository(TradezillaContext context)
        {
            _context = context;
        }

        public void RegisterOrder(Order order)
        {
            var orderModel = OrderMapper.ToModel(order);
            _context.Orders.Add(orderModel);
        }
    }
}
