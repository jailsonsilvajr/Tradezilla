using Application.Ports.Driven.Mediator;
using Domain.Entities;

namespace Application.Notifications
{
    public class PlaceOrderNotification : INotification
    {
        public Order Order { get; set; }

        public PlaceOrderNotification(Order order)
        {
            Order = order;
        }
    }
}
