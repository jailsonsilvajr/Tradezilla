using Domain.Entities;
using Domain.Enums;
using FluentAssertions;

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
            asset.GetAccountId().Should().Be(accountId);
            asset.GetAssetName().Should().Be(assetName);
            asset.GetBalance().Should().Be(0);
            asset.Transactions.Should().BeEmpty();
        }

        [Fact]
        public void AddDeposit_ShouldAddDepositAndUpdateBalance()
        {
            // Arrange
            var accountId = Guid.NewGuid();
            var asset = Asset.Create(accountId, "BTC");
            var deposit = Transaction.Create(asset.GetId(), 5, TransactionType.Credit);

            // Act
            asset.AddTransaction(deposit);

            // Assert
            asset.Transactions.Should().ContainSingle();
            asset.Transactions.First().Should().Be(deposit);
            asset.GetBalance().Should().Be(5);
        }

        [Fact]
        public void AddMultipleDeposits_ShouldUpdateBalanceCorrectly()
        {
            // Arrange
            var accountId = Guid.NewGuid();
            var asset = Asset.Create(accountId, "BTC");
            var deposit1 = Transaction.Create(asset.GetId(), 5, TransactionType.Credit);
            var deposit2 = Transaction.Create(asset.GetId(), 2, TransactionType.Credit);

            // Act
            asset.AddTransaction(deposit1);
            asset.AddTransaction(deposit2);

            // Assert
            asset.Transactions.Should().HaveCount(2);
            asset.GetBalance().Should().Be(7);
        }
    }
} 