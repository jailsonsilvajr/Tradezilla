using Domain.Exceptions;
using Domain.ValueObjects;
using FluentAssertions;

namespace Tests.Unit.ValueObjects
{
    public class NameTests
    {
        [Theory]
        [InlineData("John Doe")]
        [InlineData("John Doe Silva")]
        [InlineData("Pedro de Alcântara Francisco Antônio João Carlos Xavier de Paula Miguel Rafael Joaquim José Gonzaga")]
        public void ShouldCreateName(string testName)
        {
            var name = new Name(testName);

            name.Should().NotBeNull();
            name.GetValue().Should().Be(testName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("John")]
        [InlineData("Pedro de Alcântara Francisco Antônio João Carlos Xavier de Paula Miguel Rafael Joaquim José Gonzaga X")]
        public void ShouldThrowValidationException(string? testName)
        {
            var action = () => new Name(testName!);
            action.Should().Throw<ValidationException>();
        }
    }
}
