using Domain.Exceptions;
using Domain.ValueObjects;
using FluentAssertions;

namespace Tests.Unit.ValueObjects
{
    public class BalanceTests
    {
        [Theory]
        [InlineData(-1)]
        [InlineData(-0.01)]
        public void ShouldNotCreateNegativeBalance(decimal invalidBalance)
        {
            // Act & Assert
            var action = () => new Balance(invalidBalance);
            action.Should().Throw<ValidationException>()
                .WithMessage("Invalid balance value");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(100)]
        [InlineData(123.45)]
        public void ShouldCreateValidBalance(decimal validBalance)
        {
            // Act
            var balance = new Balance(validBalance);
            // Assert
            balance.GetValue().Should().Be(validBalance);
        }
    }
}
