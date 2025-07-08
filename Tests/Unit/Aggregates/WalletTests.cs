using Domain.Aggregates;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using FluentAssertions;

namespace Tests.Unit.Aggregates
{
    public class WalletTests
    {
        [Fact]
        public void AddOrder_WithSufficientBalance_ShouldAddOrder()
        {
            // Arrange
            var accountId = Guid.NewGuid();

            var asset = Asset.Create(accountId, "USD");
            var transaction = Transaction.Create(asset.GetId(), 1000, TransactionType.Credit);
            var wallet = new Wallet(accountId, new() { asset }, new(), new() { transaction });
            var order = Order.Create(
                accountId,
                "BTC/USD",
                "Buy",
                1,
                200);

            // Act
            wallet.AddOrder(order);

            // Assert
            wallet.Orders.Should().ContainSingle();
            wallet.Orders.First().Should().Be(order);
        }

        [Fact]
        public void AddOrder_WithNonexistentAsset_ShouldThrowEntityNotFoundException()
        {
            var accountId = Guid.NewGuid();

            var order = Order.Create(
                accountId,
                "BTC/USD",
                "Buy",
                100,
                200);

            var wallet = new Wallet(accountId, new(), new(), new());

            Assert.Throws<EntityNotFoundException>(() => wallet.AddOrder(order));
        }

        [Fact]
        public void WithoutBalance_ThrowsInsufficientBalanceException()
        {
            var accountId = Guid.NewGuid();

            var asset = Asset.Create(accountId, "USD");
            var transaction = Transaction.Create(asset.GetId(), 10, TransactionType.Credit);

            var order = Order.Create(
                accountId,
                "BTC/USD",
                "Buy",
                100,
                200);

            var wallet = new Wallet(accountId, new() { asset }, new(), new() { transaction });

            Assert.Throws<InsufficientBalanceException>(() => wallet.AddOrder(order));
        }

        [Fact]
        public void AddAsset_ShouldAddAssetToCollection()
        {
            var accountId = Guid.NewGuid();

            var asset = Asset.Create(accountId, "BTC");

            var wallet = new Wallet(accountId, new(), new(), new());
            wallet.AddAsset(asset);

            wallet.Assets.Should().ContainSingle();
            wallet.Assets.First().Should().Be(asset);
        }

        [Fact]
        public void AddDeposit_ShouldAddDepositAndUpdateBalance()
        {
            // Arrange
            var accountId = Guid.NewGuid();
            var asset = Asset.Create(accountId, "BTC");
            var deposit = Transaction.Create(asset.GetId(), 5, TransactionType.Credit);

            // Act
            var wallet = new Wallet(accountId, new() { asset }, new(), new() { deposit });

            // Assert
            wallet.Transactions.Should().ContainSingle();
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
            var wallet = new Wallet(accountId, new() { asset }, new(), new() { deposit1, deposit2 });

            // Assert
            wallet.Transactions.Should().HaveCount(2);
            asset.GetBalance().Should().Be(7);
        }
    }
}
