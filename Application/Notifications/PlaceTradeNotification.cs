using Application.Ports.Driven.Mediator;
using Domain.Entities;

namespace Application.Notifications
{
    public class PlaceTradeNotification : INotification
    {
        public Order HighestBuy { get; set; }
        public Order LowestSell { get; set; }

        public PlaceTradeNotification(Order highestBuy, Order lowestSell)
        {
            HighestBuy = highestBuy;
            LowestSell = lowestSell;
        }
    }
}
