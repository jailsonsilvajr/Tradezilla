using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using FluentAssertions;

namespace Tests.Unit.Entities
{
    public class TransactionTests
    {
        [Fact]
        public void Create_WithValidData_ShouldCreateTransaction()
        {
            // Arrange
            var assetId = Guid.NewGuid();
            var value = 5;

            // Act
            var transaction = Transaction.Create(assetId, value, TransactionType.Credit);

            // Assert
            transaction.Should().NotBeNull();
            transaction.GetTransactionId().Should().NotBeEmpty();
            transaction.GetAssetId().Should().Be(assetId);
            transaction.GetQuantity().Should().Be(value);
        }

        [Fact]
        public void Create_WithEmptyAssetId_ShouldThrowValidationException()
        {
            // Arrange
            var assetId = Guid.Empty;
            var value = 5;

            // Act & Assert
            var action = () => Transaction.Create(assetId, value, TransactionType.Credit);
            action.Should().Throw<ValidationException>();
        }

        [Fact]
        public void Create_WithInvalidValue_ShouldThrowValidationException()
        {
            // Arrange
            var assetId = Guid.NewGuid();

            // Act & Assert
            var action = () => Transaction.Create(assetId, 0, TransactionType.Credit);
            action.Should().Throw<ValidationException>();
        }
    }
} 