using Application.DTOs;
using Domain.Entities;
using Domain.Exceptions;
using FluentAssertions;

namespace Tests.Unit.Entities
{
    public class OrderTests
    {
        [Fact]
        public void Create_WithValidData_ShouldCreateOrder()
        {
            var accountId = Guid.NewGuid();
            var market = "BTC/USD";
            var side = "buy";
            var quantity = 1;
            var price = 1000m;

            var order = Order.Create(accountId, market, side, quantity, price);

            order.Should().NotBeNull();
            order.GetOrderId().Should().NotBe(Guid.Empty);
            order.GetAccountId().Should().Be(accountId);
            order.GetMarket().Should().Be(market);
            order.GetSide().Should().Be(side);
            order.Quantity.Should().Be(quantity);
            order.Price.Should().Be(price);
            order.Status.Should().Be("open");
            order.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
            order.FillQuantity.Should().Be(0);
            order.FillPrice.Should().Be(0);
        }

        [Theory]
        [InlineData("BTC/USD", "buy", 0, 1000)] // Quantidade zero
        [InlineData("BTC/USD", "buy", -1, 1000)] // Quantidade negativa
        [InlineData("BTC/USD", "buy", 1, 0)] // Preço zero
        [InlineData("BTC/USD", "buy", 1, -1)] // Preço negativo
        public void Create_WithInvalidData_ShouldThrowValidationException(
            string market, 
            string side,
            int quantity, 
            decimal price)
        {
            var accountId = Guid.NewGuid();

            var action = () => Order.Create(accountId, market, side, quantity, price);

            action.Should().Throw<ValidationException>()
                .WithMessage("Invalid data to create order");
        }

        [Theory]
        [InlineData("buy")]
        [InlineData("sell")]
        public void Create_WithValidSides_ShouldCreateOrder(string side)
        {
            var accountId = Guid.NewGuid();
            var market = "BTC/USD";
            var quantity = 1;
            var price = 1000m;

            var order = Order.Create(accountId, market, side, quantity, price);

            order.Should().NotBeNull();
            order.GetSide().Should().Be(side);
        }

        [Fact]
        public void Create_ShouldSetInitialFillValues()
        {
            var accountId = Guid.NewGuid();
            var market = "BTC/USD";
            var side = "buy";
            var quantity = 1;
            var price = 1000m;

            var order = Order.Create(accountId, market, side, quantity, price);

            order.FillQuantity.Should().Be(0);
            order.FillPrice.Should().Be(0);
        }

        [Fact]
        public void Create_ShouldSetInitialStatusAsOpen()
        {
            var accountId = Guid.NewGuid();
            var market = "BTC/USD";
            var side = "buy";
            var quantity = 1;
            var price = 1000m;

            var order = Order.Create(accountId, market, side, quantity, price);

            order.Status.Should().Be("open");
        }

        [Fact]
        public void GroupOrder_WithPrecision0()
        {
            var accountId = Guid.NewGuid();
            var orders = new List<Order>()
            {
                Order.Create(accountId, "BTC/USD", "sell", 1, 94550),
                Order.Create(accountId, "BTC/USD", "sell", 2, 94500),
                Order.Create(accountId, "BTC/USD", "buy", 1, 94600)
            };

            var index = Order.GroupOrdersByPrecision(orders, 0);

            index.Should().NotBeNull();

            index.buy.Should().NotBeNull();
            index.sell.Should().NotBeNull();

            index.buy.Count.Should().Be(1);
            index.buy["94600"].Should().Be(1);

            index.sell["94550"].Should().Be(1);
            index.sell["94500"].Should().Be(2);
        }

        [Fact]
        public void GroupOrder_WithPrecision1()
        {
            var accountId = Guid.NewGuid();
            var orders = new List<Order>()
            {
                Order.Create(accountId, "BTC/USD", "sell", 1, 94550),
                Order.Create(accountId, "BTC/USD", "sell", 2, 94500),
                Order.Create(accountId, "BTC/USD", "buy", 1, 94600)
            };

            var index = Order.GroupOrdersByPrecision(orders, 1);

            index.Should().NotBeNull();

            index.buy.Should().NotBeNull();
            index.sell.Should().NotBeNull();

            index.buy.Count.Should().Be(1);
            index.buy["94600"].Should().Be(1);

            index.sell["94550"].Should().Be(1);
            index.sell["94500"].Should().Be(2);
        }

        [Fact]
        public void GroupOrder_WithPrecision2()
        {
            var accountId = Guid.NewGuid();
            var orders = new List<Order>()
            {
                Order.Create(accountId, "BTC/USD", "sell", 1, 94550),
                Order.Create(accountId, "BTC/USD", "sell", 2, 94500),
                Order.Create(accountId, "BTC/USD", "buy", 1, 94600)
            };

            var index = Order.GroupOrdersByPrecision(orders, 2);

            index.Should().NotBeNull();

            index.buy.Should().NotBeNull();
            index.sell.Should().NotBeNull();

            index.buy.Count.Should().Be(1);
            index.buy["94600"].Should().Be(1);

            index.sell["94500"].Should().Be(3);
        }

        [Fact]
        public void GroupOrder_WithPrecision3()
        {
            var accountId = Guid.NewGuid();
            var orders = new List<Order>()
            {
                Order.Create(accountId, "BTC/USD", "sell", 1, 94550),
                Order.Create(accountId, "BTC/USD", "sell", 2, 94500),
                Order.Create(accountId, "BTC/USD", "buy", 1, 94600)
            };

            var index = Order.GroupOrdersByPrecision(orders, 3);

            index.Should().NotBeNull();

            index.buy.Should().NotBeNull();
            index.sell.Should().NotBeNull();

            index.buy.Count.Should().Be(1);
            index.buy["94000"].Should().Be(1);

            index.sell["94000"].Should().Be(3);
        }
    }
}
