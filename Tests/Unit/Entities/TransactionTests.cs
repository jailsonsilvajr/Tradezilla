using Domain.Entities;
using Domain.Exceptions;
using FluentAssertions;
using Xunit;

namespace Tests.Unit.Entities
{
    public class TransactionTests
    {
        [Fact]
        public void Create_WithValidData_ShouldCreateTransaction()
        {
            // Arrange
            var assetId = Guid.NewGuid();
            var value = 1.5m;

            // Act
            var transaction = Transaction.Create(assetId, value);

            // Assert
            transaction.Should().NotBeNull();
            transaction.TransactionId.Should().NotBeEmpty();
            transaction.AssetId.Should().Be(assetId);
            transaction.Value.Should().Be(value);
        }

        [Fact]
        public void Create_WithEmptyAssetId_ShouldThrowValidationException()
        {
            // Arrange
            var assetId = Guid.Empty;
            var value = 1.5m;

            // Act & Assert
            var action = () => Transaction.Create(assetId, value);
            action.Should().Throw<ValidationException>()
                .WithMessage("Invalid data to create transaction");
        }

        [Fact]
        public void Create_WithInvalidValue_ShouldThrowValidationException()
        {
            // Arrange
            var assetId = Guid.NewGuid();

            // Act & Assert
            var action = () => Transaction.Create(assetId, 0);
            action.Should().Throw<ValidationException>()
                .WithMessage("Invalid data to create transaction");
        }

        [Fact]
        public void Create_WithValidValue_ShouldAcceptDecimalValues()
        {
            // Arrange
            var assetId = Guid.NewGuid();
            var value = 0.00000001m; // Teste com valor decimal muito pequeno

            // Act
            var transaction = Transaction.Create(assetId, value);

            // Assert
            transaction.Should().NotBeNull();
            transaction.Value.Should().Be(value);
        }
    }
} 