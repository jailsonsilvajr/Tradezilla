using Domain.Aggregates;
using FluentAssertions;

namespace Tests.Unit.Aggregates
{
    public class UserTests
    {
        [Fact]
        public void Create_WithValidData_ShouldCreateAccount()
        {
            var name = "John Doe";
            var email = "johndoe@gmail.com";
            var document = "58865866012";
            var password = "asdQWE123";

            var user = User.Create(name, email, document, password);

            user.Should().NotBeNull();
            user.GetAccountId().Should().NotBe(Guid.Empty);
            user.GetName().Should().Be(name);
            user.GetEmail().Should().Be(email);
            user.GetDocument().Should().Be("58865866012");
            user.GetPassword().Should().Be(password);
        }
    }
}
