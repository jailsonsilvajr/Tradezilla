using Domain.Entities;
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
            var document = "588.658.660-12";
            var password = "asdQWE123";

            var account = Account.Create(name, email, document, password);

            account.Should().NotBeNull();
            account.AccountId.Should().NotBe(Guid.Empty);
            account.Name.Should().Be(name);
            account.Email.Should().Be(email);
            account.Document.Should().Be("58865866012");
            account.Password.Should().Be(password);
            account.Assets.Should().BeEmpty();
            account.Orders.Should().BeEmpty();
        }

        [Theory]
        [InlineData(null, "email@test.com", "12345678901", "password123")]
        [InlineData("John Doe", null, "12345678901", "password123")]
        [InlineData("John Doe", "email@test.com", null, "password123")]
        [InlineData("John Doe", "email@test.com", "12345678901", null)]
        public void Create_WithInvalidData_ShouldThrowValidationException(string? name, string? email, string? document, string? password)
        {
            var action = () => Account.Create(name, email, document, password);
            action.Should().Throw<ValidationException>();
        }

        [Theory]
        [InlineData("123.456.789-01", "12345678901")]
        [InlineData("123456789-01", "12345678901")]
        [InlineData("12345678901", "12345678901")]
        [InlineData(null, null)]
        public void CleanDocument_ShouldRemoveSpecialCharacters(string? input, string? expected)
        {
            var result = Account.CleanDocument(input);

            result.Should().Be(expected);
        }

        [Fact]
        public void AddAsset_ShouldAddAssetToCollection()
        {
            var account = Account.Create(
                "John Doe",
                "johndoe@gmail.com",
                "58865866012",
                "asdQWE123");
            var asset = Asset.Create(account.AccountId, "BTC");

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

            var asset = Asset.Create(account.AccountId, "USD");
            var deposit = Deposit.Create(asset.AssetId, 1000);
            asset.AddDeposit(deposit);
            account.AddAsset(asset);

            var order = Order.Create(
                account.AccountId,
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

            var asset = Asset.Create(account.AccountId, "USD");
            var deposit = Deposit.Create(asset.AssetId, 10);
            asset.AddDeposit(deposit);
            account.AddAsset(asset);

            var order = Order.Create(
                account.AccountId,
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
                account.AccountId,
                "BTC/USD",
                "Buy",
                100,
                200);

            Assert.Throws<EntityNotFoundException>(() => account.AddOrder(order));
        }
    }
}
