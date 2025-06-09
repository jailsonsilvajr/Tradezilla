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
            var account = Account.Create(
                "John Doe",
                "johndoe@gmail.com",
                "58865866012",
                "asdQWE123");

            var asset = Asset.Create(account.GetId(), "USD");
            var transaction = Transaction.Create(asset.GetId(), 1000, TransactionType.Credit);
            var wallet = new Wallet(account.GetId(), new() { asset }, new(), new() { transaction });
            var order = Order.Create(
                account.GetId(),
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
            var account = Account.Create(
                "John Doe",
                "johndoe@gmail.com",
                "58865866012",
                "asdQWE123");

            var order = Order.Create(
                account.GetId(),
                "BTC/USD",
                "Buy",
                100,
                200);

            var wallet = new Wallet(account.GetId(), new(), new(), new());

            Assert.Throws<EntityNotFoundException>(() => wallet.AddOrder(order));
        }

        [Fact]
        public void WithoutBalance_ThrowsInsufficientBalanceException()
        {
            var account = Account.Create(
                name: "John Doe",
                email: "johndoe@gmail.com",
                document: "58865866012",
                password: "asdQWE123");

            var asset = Asset.Create(account.GetId(), "USD");
            var transaction = Transaction.Create(asset.GetId(), 10, TransactionType.Credit);

            var order = Order.Create(
                account.GetId(),
                "BTC/USD",
                "Buy",
                100,
                200);

            var wallet = new Wallet(account.GetId(), new() { asset }, new(), new() { transaction });

            Assert.Throws<InsufficientBalanceException>(() => wallet.AddOrder(order));
        }

        [Fact]
        public void AddAsset_ShouldAddAssetToCollection()
        {
            var account = Account.Create(
                "John Doe",
                "johndoe@gmail.com",
                "58865866012",
                "asdQWE123");
            var asset = Asset.Create(account.GetId(), "BTC");

            var wallet = new Wallet(account.GetId(), new(), new(), new());
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
