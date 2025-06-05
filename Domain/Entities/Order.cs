using Domain.Exceptions;
using Domain.Validators;
using Domain.ValueObjects;

namespace Domain.Entities
{
    public class Order
    {
        private readonly ID _orderId;
        private readonly ID _accountId;
        private readonly Market _market;
        private readonly Side _side;
        private readonly Quantity _quantity;
        private readonly Price _price;
        private readonly Date _createdDate;
        private Status _status;

        public int FillQuantity { get; private set; }
        public decimal FillPrice { get; private set; }
        public Account? Account { get; set; }

        public Order(
            Guid orderId, Guid accountId, string market, 
            string side, int quantity, decimal price, 
            DateTime createdTime, string status)
        {
            _orderId = new ID(orderId);
            _accountId = new ID(accountId);
            _market = new Market(market);
            _side = new Side(side);
            _quantity = new Quantity(quantity);
            _price = new Price(price);
            _createdDate = new Date(createdTime);
            _status = new Status(status);
        }

        public Guid GetOrderId() => _orderId.GetValue();
        public Guid GetAccountId() => _accountId.GetValue();
        public string GetMarket() => _market.GetValue();
        public string GetSide() => _side.GetValue();
        public int GetQuantity() => _quantity.GetValue();
        public decimal GetPrice() => _price.GetValue();
        public DateTime GetCreatedDate() => _createdDate.GetValue();
        public string GetStatus() => _status.GetValue();

        public static Order Create(
            Guid accountId, string market, string side,
            int quantity, decimal price)
        {
            var newOrder = new Order(
                Guid.NewGuid(),
                accountId,
                market,
                side,
                quantity,
                price,
                DateTime.UtcNow,
                "open");

            return newOrder;
        }

        public static (Dictionary<string, int> buy, Dictionary<string, int> sell) GroupOrdersByPrecision(List<Order>? orders, int precision)
        {
            (Dictionary<string, int> buy, Dictionary<string, int> sell) index = new(new Dictionary<string, int>(), new Dictionary<string, int>());
            foreach (var order in orders ?? new())
            {
                var price = order.GetPrice();
                if (precision > 0)
                {
                    price -= price % (decimal)Math.Pow(10, precision);
                }

                if (order.GetSide().ToUpper() == "BUY")
                {
                    if (!index.buy.ContainsKey(price.ToString()))
                    {
                        index.buy[price.ToString()] = 0;
                    }

                    index.buy[price.ToString()] += order.GetQuantity();
                }

                if (order.GetSide().ToUpper() == "SELL")
                {
                    if (!index.sell.ContainsKey(price.ToString()))
                    {
                        index.sell[price.ToString()] = 0;
                    }

                    index.sell[price.ToString()] += order.GetQuantity();
                }
            }

            return index;
        }

        public void SetFillQuantity(int quantity)
        {
            if (quantity < 0)
            {
                throw new ArgumentException("Fill quantity cannot be negative");
            }

            FillQuantity = quantity;

            if (FillQuantity == GetQuantity())
            {
                _status = new Status("closed");
            }
        }

        public void SetFillPrice(decimal price)
        {
            if (price < 0)
            {
                throw new ArgumentException("Fill price cannot be negative");
            }

            FillPrice = price;
        }
    }
}
