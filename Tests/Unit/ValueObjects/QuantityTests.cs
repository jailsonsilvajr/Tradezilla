using Domain.Exceptions;
using Domain.ValueObjects;
using FluentAssertions;

namespace Tests.Unit.ValueObjects
{
    public class QuantityTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void ShouldNotCreateQuantity(int invalidValue)
        {
            Action action = () => new Quantity(invalidValue);

            action.Should().Throw<ValidationException>();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(1000)]
        public void ShouldCreateQuantity(int validValue)
        {
            var quantity = new Quantity(validValue);

            quantity.Should().NotBeNull();
            quantity.GetValue().Should().Be(validValue);
        }
    }
}
