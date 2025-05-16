using Domain.Entities;
using Domain.Exceptions;
using FluentAssertions;
using Xunit;

namespace Tests.Unit.Entities
{
    public class DepositTests
    {
        [Fact]
        public void Create_WithValidData_ShouldCreateDeposit()
        {
            // Arrange
            var assetId = Guid.NewGuid();
            var quantity = 1.5m;

            // Act
            var deposit = Deposit.Create(assetId, quantity);

            // Assert
            deposit.Should().NotBeNull();
            deposit.DepositId.Should().NotBeEmpty();
            deposit.AssetId.Should().Be(assetId);
            deposit.Quantity.Should().Be(quantity);
        }

        [Fact]
        public void Create_WithEmptyAssetId_ShouldThrowValidationException()
        {
            // Arrange
            var assetId = Guid.Empty;
            var quantity = 1.5m;

            // Act & Assert
            var action = () => Deposit.Create(assetId, quantity);
            action.Should().Throw<ValidationException>()
                .WithMessage("Invalid data to create deposit");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-100)]
        public void Create_WithInvalidQuantity_ShouldThrowValidationException(decimal invalidQuantity)
        {
            // Arrange
            var assetId = Guid.NewGuid();

            // Act & Assert
            var action = () => Deposit.Create(assetId, invalidQuantity);
            action.Should().Throw<ValidationException>()
                .WithMessage("Invalid data to create deposit");
        }

        [Fact]
        public void Create_WithValidQuantity_ShouldAcceptDecimalValues()
        {
            // Arrange
            var assetId = Guid.NewGuid();
            var quantity = 0.00000001m; // Teste com valor decimal muito pequeno

            // Act
            var deposit = Deposit.Create(assetId, quantity);

            // Assert
            deposit.Should().NotBeNull();
            deposit.Quantity.Should().Be(quantity);
        }
    }
} 