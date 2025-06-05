using Domain.Exceptions;
using Domain.ValueObjects;
using FluentAssertions;

namespace Tests.Unit.ValueObjects
{
    public class StatusTests
    {
        [Theory]
        [InlineData("open")]
        [InlineData("closed")]
        public void ShouldCreateStatus(string validStatus)
        {
            var status = new Status(validStatus);
            status.GetValue().Should().Be(validStatus);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(" ")]
        [InlineData("")]
        [InlineData("OPEN")]
        [InlineData("CLOSED")]
        public void ShouldNotCreateStatus(string? invalidStatus)
        {
            var action = () => new Status(invalidStatus!);

            action.Should().Throw<ValidationException>();
        }
    }
}
