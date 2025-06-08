using Domain.Entities;
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
    }
}
