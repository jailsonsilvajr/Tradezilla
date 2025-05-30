using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using FluentAssertions;
using Xunit;

namespace Tests.Unit.Entities
{
    public class AssetTests
    {
        [Fact]
        public void Create_WithValidData_ShouldCreateAsset()
        {
            // Arrange
            var accountId = Guid.NewGuid();
            var assetName = "BTC";

            // Act
            var asset = Asset.Create(accountId, assetName);

            // Assert
            asset.Should().NotBeNull();
            asset.AccountId.Should().Be(accountId);
            asset.AssetName.Should().Be(assetName);
            asset.Balance.Should().Be(0);
            asset.Transactions.Should().BeEmpty();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("BITCOIN")] // Maior que MAX_ASSETNAME_LENGTH (5)
        public void Create_WithInvalidAssetName_ShouldThrowValidationException(string? invalidAssetName)
        {
            // Arrange
            var accountId = Guid.NewGuid();

            // Act & Assert
            var action = () => Asset.Create(accountId, invalidAssetName);
            action.Should().Throw<ValidationException>()
                .WithMessage("Invalid data to create asset");
        }

        [Fact]
        public void Create_WithEmptyAccountId_ShouldThrowValidationException()
        {
            // Arrange
            var accountId = Guid.Empty;
            var assetName = "BTC";

            // Act & Assert
            var action = () => Asset.Create(accountId, assetName);
            action.Should().Throw<ValidationException>()
                .WithMessage("Invalid data to create asset");
        }

        [Fact]
        public void AddDeposit_ShouldAddDepositAndUpdateBalance()
        {
            // Arrange
            var accountId = Guid.NewGuid();
            var asset = Asset.Create(accountId, "BTC");
            var deposit = Transaction.Create(asset.GetId(), 1.5m, TransactionType.Credit);

            // Act
            asset.AddTransaction(deposit);

            // Assert
            asset.Transactions.Should().ContainSingle();
            asset.Transactions.First().Should().Be(deposit);
            asset.Balance.Should().Be(1.5m);
        }

        [Fact]
        public void AddMultipleDeposits_ShouldUpdateBalanceCorrectly()
        {
            // Arrange
            var accountId = Guid.NewGuid();
            var asset = Asset.Create(accountId, "BTC");
            var deposit1 = Transaction.Create(asset.GetId(), 1.5m, TransactionType.Credit);
            var deposit2 = Transaction.Create(asset.GetId(), 2.5m, TransactionType.Credit);

            // Act
            asset.AddTransaction(deposit1);
            asset.AddTransaction(deposit2);

            // Assert
            asset.Transactions.Should().HaveCount(2);
            asset.Balance.Should().Be(4.0m);
        }
    }
} 