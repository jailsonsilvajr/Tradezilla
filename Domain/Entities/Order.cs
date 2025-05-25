using Domain.Exceptions;
using Domain.Validators;

namespace Domain.Entities
{
    public class Order
    {
        public static readonly int MAX_MARKET_LENGTH = 7;
        public static readonly int MAX_SIDE_LENGTH = 5;
        public static readonly int MAX_STATUS_LENGTH = 10;
        private static readonly OrderValidator _validator = new OrderValidator();
        public Guid OrderId { get; }
        public Guid AccountId { get; set; }
        public string? Market { get; }
        public string? Side { get; }
        public int Quantity { get; }
        public decimal Price { get; }
        public int FillQuantity { get; private set; }
        public decimal FillPrice { get; private set; }
        public DateTime CreatedAt { get; }
        public string? Status { get; set; }
        public Account? Account { get; set; }

        public Order(
            Guid orderId, Guid accountId, string? market, 
            string? side, int quantity, decimal price, 
            DateTime createdAt, string? status)
        {
            OrderId = orderId;
            AccountId = accountId;
            Market = market;
            Side = side;
            Quantity = quantity;
            Price = price;
            CreatedAt = createdAt;
            Status = status;

            Validate(this);
        }

        public static Order Create(
            Guid accountId, string? market, string? side,
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

            Validate(newOrder);
            return newOrder;
        }

        private static void Validate(Order order)
        {
            var validationResult = _validator.Validate(order);
            if (!validationResult.IsValid)
            {
                throw new ValidationException("Invalid data to create order", validationResult.Errors);
            }
        }

        public static (Dictionary<string, int> buy, Dictionary<string, int> sell) GroupOrdersByPrecision(List<Order>? orders, int precision)
        {
            (Dictionary<string, int> buy, Dictionary<string, int> sell) index = new(new Dictionary<string, int>(), new Dictionary<string, int>());
            foreach (var order in orders ?? new())
            {
                var price = order.Price;
                if (precision > 0)
                {
                    price -= price % (decimal)Math.Pow(10, precision);
                }

                if (order.Side?.ToUpper() == "BUY")
                {
                    if (!index.buy.ContainsKey(price.ToString()))
                    {
                        index.buy[price.ToString()] = 0;
                    }

                    index.buy[price.ToString()] += order.Quantity;
                }

                if (order.Side?.ToUpper() == "SELL")
                {
                    if (!index.sell.ContainsKey(price.ToString()))
                    {
                        index.sell[price.ToString()] = 0;
                    }

                    index.sell[price.ToString()] += order.Quantity;
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

            if (FillQuantity == Quantity)
            {
                Status = "closed";
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
