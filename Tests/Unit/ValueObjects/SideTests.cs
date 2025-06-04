using Domain.Exceptions;
using Domain.ValueObjects;
using FluentAssertions;

namespace Tests.Unit.ValueObjects
{
    public class SideTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("BUYYYY")]
        public void ShouldNotCreateSide(string? side)
        {
            var action = () => new Side(side!);

            action.Should().Throw<ValidationException>()
                .WithMessage("Invalid side");
        }

        [Theory]
        [InlineData("buy")]
        [InlineData("sell")]
        public void ShouldCreateSide(string side)
        {
            var sideObject = new Side(side);
            sideObject.GetValue().Should().Be(side);
        }
    }
}
