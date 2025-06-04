using Domain.Exceptions;
using Domain.ValueObjects;
using FluentAssertions;

namespace Tests.Unit.ValueObjects
{
    public class MarketTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("BTC/USDD")]
        public void ShouldNotCreateMarket(string? market)
        {
            var action = () => new Market(market!);
            action.Should().Throw<ValidationException>()
                .WithMessage("Invalid market");
        }

        [Theory]
        [InlineData("BTC/USD")]
        [InlineData("USD/BTC")]
        public void ShoulCreateMarket(string? validMarket)
        {
            var market = new Market(validMarket!);

            market.Should().NotBeNull();
            market.GetValue().Should().NotBeNullOrWhiteSpace();
            market.GetValue().Should().Be(validMarket);
        }
    }
}
