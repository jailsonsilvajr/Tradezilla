using Domain.Entities;

namespace Application.Ports.Driven
{
    public interface IOrderRepository
    {
        void RegisterOrder(Order order);
    }
}
