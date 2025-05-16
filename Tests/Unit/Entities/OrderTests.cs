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
            order.OrderId.Should().NotBe(Guid.Empty);
            order.AccountId.Should().Be(accountId);
            order.Market.Should().Be(market);
            order.Side.Should().Be(side);
            order.Quantity.Should().Be(quantity);
            order.Price.Should().Be(price);
            order.Status.Should().Be("open");
            order.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
            order.FillQuantity.Should().Be(0);
            order.FillPrice.Should().Be(0);
        }

        [Fact]
        public void Create_WithEmptyAccountId_ShouldThrowValidationException()
        {
            var accountId = Guid.Empty;
            var market = "BTC/USD";
            var side = "buy";
            var quantity = 1;
            var price = 1000m;

            var action = () => Order.Create(accountId, market, side, quantity, price);

            action.Should().Throw<ValidationException>()
                .WithMessage("Invalid data to create order");
        }

        [Theory]
        [InlineData(null, "buy", 1, 1000)]
        [InlineData("", "buy", 1, 1000)]
        [InlineData(" ", "buy", 1, 1000)]
        [InlineData("BTCUSDT/T", "buy", 1, 1000)] // Excede MAX_MARKET_LENGTH
        [InlineData("BTC/USD", null, 1, 1000)]
        [InlineData("BTC/USD", "", 1, 1000)]
        [InlineData("BTC/USD", " ", 1, 1000)]
        [InlineData("BTC/USD", "BUYYYY", 1, 1000)] // Excede MAX_SIDE_LENGTH
        [InlineData("BTC/USD", "invalid", 1, 1000)] // Side inválido
        [InlineData("BTC/USD", "buy", 0, 1000)] // Quantidade zero
        [InlineData("BTC/USD", "buy", -1, 1000)] // Quantidade negativa
        [InlineData("BTC/USD", "buy", 1, 0)] // Preço zero
        [InlineData("BTC/USD", "buy", 1, -1)] // Preço negativo
        public void Create_WithInvalidData_ShouldThrowValidationException(
            string? market, 
            string? side,
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
            order.Side.Should().Be(side);
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
    }
}
