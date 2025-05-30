using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using FluentAssertions;

namespace Tests.Unit.Entities
{
    public class AccountTests
    {
        [Fact]
        public void Create_WithValidData_ShouldCreateAccount()
        {
            var name = "John Doe";
            var email = "johndoe@gmail.com";
            var document = "58865866012";
            var password = "asdQWE123";

            var account = Account.Create(name, email, document, password);

            account.Should().NotBeNull();
            account.GetId().Should().NotBe(Guid.Empty);
            account.GetName().Should().Be(name);
            account.GetEmail().Should().Be(email);
            account.GetDocument().Should().Be("58865866012");
            account.GetPassword().Should().Be(password);
            account.Assets.Should().BeEmpty();
            account.Orders.Should().BeEmpty();
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

            account.AddAsset(asset);

            account.Assets.Should().ContainSingle();
            account.Assets.First().Should().Be(asset);
        }

        [Fact]
        public void AddOrder_WithSufficientBalance_ShouldAddOrder()
        {
            var account = Account.Create(
                "John Doe",
                "johndoe@gmail.com",
                "58865866012",
                "asdQWE123");

            var asset = Asset.Create(account.GetId(), "USD");
            var transaction = Transaction.Create(asset.GetId(), 1000, TransactionType.Credit);
            asset.AddTransaction(transaction);
            account.AddAsset(asset);

            var order = Order.Create(
                account.GetId(),
                "BTC/USD",
                "Buy",
                1,
                200);

            account.AddOrder(order);

            account.Orders.Should().ContainSingle();
            account.Orders.First().Should().Be(order);
        }

        [Fact]
        public void AccountWithoutBalance_ThrowsInsufficientBalanceException()
        {
            var account = Account.Create(
                name: "John Doe",
                email: "johndoe@gmail.com",
                document: "58865866012",
                password: "asdQWE123");

            var asset = Asset.Create(account.GetId(), "USD");
            var transaction = Transaction.Create(asset.GetId(), 10, TransactionType.Credit);
            asset.AddTransaction(transaction);
            account.AddAsset(asset);

            var order = Order.Create(
                account.GetId(),
                "BTC/USD",
                "Buy",
                100,
                200);

            Assert.Throws<InsufficientBalanceException>(() => account.AddOrder(order));
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

            Assert.Throws<EntityNotFoundException>(() => account.AddOrder(order));
        }
    }
}
