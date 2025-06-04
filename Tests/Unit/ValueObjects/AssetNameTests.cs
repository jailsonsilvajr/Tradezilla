using Domain.Exceptions;
using Domain.ValueObjects;
using FluentAssertions;

namespace Tests.Unit.ValueObjects
{
    public class AssetNameTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("123456")]
        public void ShouldNotCreateAssetName(string? invalidAssetName)
        {
            // Act & Assert
            var action = () => new AssetName(invalidAssetName!);
            action.Should().Throw<ValidationException>()
                .WithMessage("Invalid asset name");
        }

        [Theory]
        [InlineData("BTC")]
        [InlineData("USD")]
        public void ShouldCreateValidAssetName(string validAssetName)
        {
            // Act
            var assetName = new AssetName(validAssetName);
            // Assert
            assetName.GetValue().Should().Be(validAssetName);
        }
    }
}
