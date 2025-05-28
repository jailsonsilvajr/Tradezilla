using Domain.Exceptions;
using Domain.ValueObjects;
using FluentAssertions;

namespace Tests.Unit.ValueObjects
{
    public class DocumentTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("111")]
        [InlineData("abc")]
        [InlineData("7897897897")]
        public void ShouldThrowValidationException(string? document)
        {
            var action = () => new Document(document!);
            action.Should().Throw<ValidationException>();
        }
    }
}
