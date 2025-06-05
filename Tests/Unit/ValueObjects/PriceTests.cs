using Domain.Exceptions;
using Domain.ValueObjects;
using FluentAssertions;

namespace Tests.Unit.ValueObjects
{
    public class PriceTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-1.2)]
        [InlineData(-1000)]
        public void ShouldNotCreatePrice(decimal invalidPrice)
        {
            var action = () => new Price(invalidPrice);
            action.Should().Throw<ValidationException>();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2.2)]
        [InlineData(1000)]
        public void ShouldCreatePrice(decimal validPrice)
        {
            var price = new Price(validPrice);

            price.Should().NotBeNull();
            price.GetValue().Should().Be(validPrice);
        }
    }
}
