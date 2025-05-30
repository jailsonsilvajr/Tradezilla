using Domain.Exceptions;
using Domain.ValueObjects;
using FluentAssertions;

namespace Tests.Unit.ValueObjects
{
    public class IDTests
    {
        [Fact]
        public void MustCreateAID()
        {
            var newId = Guid.NewGuid();
            var id = new ID(newId);

            id.Should().NotBeNull();
            id.GetValue().Should().Be(newId);
        }

        [Fact]
        public void ShouldNotCreateID()
        {
            var acation = () => new ID(Guid.Empty);
            acation.Should().Throw<ValidationException>();
        }
    }
}
