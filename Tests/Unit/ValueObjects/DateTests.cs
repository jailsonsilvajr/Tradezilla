using Domain.Exceptions;
using Domain.ValueObjects;
using FluentAssertions;

namespace Tests.Unit.ValueObjects
{
    public class DateTests
    {
        [Fact]
        public void ShouldNotCreateDate()
        {
            var action = () => new Date(DateTime.MinValue);

            action.Should().Throw<ValidationException>();
        }

        [Theory]
        [InlineData("2023-01-01")]
        [InlineData("01-01-2023")]
        [InlineData("01-01-2023 10:00:00")]
        public void ShouldCreateDate(DateTime validDate)
        {
            var date = new Date(validDate);
            
            date.Should().NotBeNull();
            date.GetValue().Should().Be(validDate);
        }
    }
}
