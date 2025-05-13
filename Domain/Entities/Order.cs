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
        public int FillQuantity { get; }
        public decimal FillPrice { get; }
        public DateTime CreatedAt { get; }
        public string? Status { get; set; }
        public Account? Account { get; set; }

        private Order(
            Guid orderId, Guid accountId, string? market, 
            string? side, int quantity, decimal price, 
            int fillQuantity, decimal fillPrice, 
            DateTime createdAt, string? status)
        {
            OrderId = orderId;
            AccountId = accountId;
            Market = market;
            Side = side;
            Quantity = quantity;
            Price = price;
            FillQuantity = fillQuantity;
            FillPrice = fillPrice;
            CreatedAt = createdAt;
            Status = status;
        }

        public static Order Create(
            Guid accountId, string? market, string? side,
            int quantity, decimal price, int fillQuantity, decimal fillPrice)
        {
            var newOrder = new Order(
                Guid.NewGuid(),
                accountId,
                market,
                side,
                quantity,
                price,
                fillQuantity,
                fillPrice,
                DateTime.UtcNow,
                "open");

            var validationResult = _validator.Validate(newOrder);
            if (!validationResult.IsValid)
            {
                throw new ValidationException("Invalid data to create order", validationResult.Errors);
            }

            return newOrder;
        }
    }
}
