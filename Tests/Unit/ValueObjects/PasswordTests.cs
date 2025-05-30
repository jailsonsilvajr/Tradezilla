using Domain.Exceptions;
using Domain.ValueObjects;
using FluentAssertions;

namespace Tests.Unit.ValueObjects
{
    public class PasswordTests
    {
        [Theory]
        [InlineData("asdQWE123")]
        [InlineData("ASDqwe123")]
        [InlineData("123asdQWE")]
        [InlineData("asd123QWE")]
        [InlineData("ASD123qwe")]
        public void ShouldCreatePassword(string passwordTest)
        {
            var passwordCreated = new Password(passwordTest);
            passwordCreated.Should().NotBeNull();
        }

        [Theory]
        [InlineData("ASDQWE123")]
        [InlineData("asdqwe123")]
        [InlineData("asdqwe")]
        [InlineData("ASDQWE")]
        [InlineData("12345678")]
        [InlineData(null)]
        public void ShouldThrowValidationException(string? password)
        {
            var action = () => new Password(password!);
            action.Should().Throw<ValidationException>();
        }
    }
}
