using Domain.Entities;
using FluentAssertions;

namespace Tests.Unit.Entities
{
    public class AssetTests
    {
        [Fact]
        public void Create_WithValidData_ShouldCreateAsset()
        {
            // Arrange
            var accountId = Guid.NewGuid();
            var assetName = "BTC";

            // Act
            var asset = Asset.Create(accountId, assetName);

            // Assert
            asset.Should().NotBeNull();
            asset.GetAccountId().Should().Be(accountId);
            asset.GetAssetName().Should().Be(assetName);
            asset.GetBalance().Should().Be(0);
        }
    }
} 