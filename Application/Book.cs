using Application.Notifications;
using Application.Ports.Driven.Mediator;
using Domain.Entities;

namespace Application
{
    public class Book : IBook
    {
        private readonly IMediator _mediator;

        public Book(IMediator mediator)
        {
            _mediator = mediator;
        }

        private List<Order> _ordersBuy = new();
        private List<Order> _ordersSell = new();

        public void AddOrder(Order order)
        {
            if (order.GetSide().ToUpper() == "BUY")
            {
                _ordersBuy.Add(order);
                var ordersSorted = _ordersBuy.OrderByDescending(o => o.GetPrice());
                _ordersBuy = ordersSorted.ToList();
            }
            else if (order.GetSide().ToUpper() == "SELL")
            {
                _ordersSell.Add(order);
                var ordersSorted = _ordersSell.OrderBy(o => o.GetPrice());
                _ordersSell = ordersSorted.ToList();
            }
        }

        public async Task MatchOrderAsync(string marketId)
        {
            var highestBuy = _ordersBuy.FirstOrDefault(x => x.GetMarket().Equals(marketId));
            var lowestSell = _ordersSell.FirstOrDefault(x => x.GetMarket().Equals(marketId));
            if (highestBuy is null || lowestSell is null || highestBuy.GetPrice() < lowestSell.GetPrice())
            {
                return;
            }

            var fillQuantity = Math.Min(highestBuy.GetQuantity(), lowestSell.GetQuantity());

            highestBuy.SetFillQuantity(fillQuantity);
            lowestSell.SetFillQuantity(fillQuantity);

            await _mediator.Send(new PlaceTradeNotification(highestBuy, lowestSell));
        }
    }
}
