using Domain.Entities;
using Domain.Exceptions;

namespace Tests.Unit.Entities
{
    public class OrderTests
    {
        [Fact]
        public void CreateOrder_ValidData_ReturnsOrder()
        {
            var accountId = Guid.NewGuid();
            var market = "BTCUSD";
            var side = "buy";
            var quantity = 1;
            var price = 1000m;
            var fillQuantity = 1;
            var fillPrice = 1000m;

            var order = Order.Create(accountId, market, side, quantity, price, fillQuantity, fillPrice);

            Assert.NotNull(order);
            Assert.Equal(accountId, order.AccountId);
            Assert.Equal(market, order.Market);
            Assert.Equal(side, order.Side);
            Assert.Equal(quantity, order.Quantity);
            Assert.Equal(price, order.Price);
            Assert.Equal(fillQuantity, order.FillQuantity);
            Assert.Equal(fillPrice, order.FillPrice);
        }

        [Fact]
        public void CreateOrder_ValidAccountId_ReturnsOrder()
        {
            var accountId = Guid.Empty;
            var market = "BTCUSD";
            var side = "buy";
            var quantity = 1;
            var price = 1000m;
            var fillQuantity = 1;
            var fillPrice = 1000m;

            Action act = () => Order.Create(accountId, market, side, quantity, price, fillQuantity, fillPrice);
            var exception = Assert.Throws<ValidationException>(act);

            Assert.Equal("Invalid data to create order", exception.Message);
        }

        [Theory]
        [InlineData(null, "buy", 1, 1000)]
        [InlineData("", "buy", 1, 1000)]
        [InlineData(" ", "buy", 1, 1000)]
        [InlineData("BTCUSDDSD", "buy", 1, 1000)]
        [InlineData("BTCUSD", null, 1, 1000)]
        [InlineData("BTCUSD", "", 1, 1000)]
        [InlineData("BTCUSD", " ", 1, 1000)]
        [InlineData("BTCUSD", "123456", 1, 1000)]
        [InlineData("BTCUSD", "buy", 0, 1000)]
        [InlineData("BTCUSD", "buy", 1, 0)]
        public void CreateOrder_InvalidData_ThrowsValidationException(
            string? market, 
            string? side,
            int quantity, 
            decimal price)
        {
            var fillQuantity = 1;
            var fillPrice = 1000m;

            Action act = () => Order.Create(Guid.NewGuid(), market, side, quantity, price, fillQuantity, fillPrice);
            var exception = Assert.Throws<ValidationException>(act);
            
            Assert.Equal("Invalid data to create order", exception.Message);
        }
    }
}
