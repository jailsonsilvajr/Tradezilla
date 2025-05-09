using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DatabaseContext.Config
{
    public class AssetConfig : IEntityTypeConfiguration<Asset>
    {
        public void Configure(EntityTypeBuilder<Asset> builder)
        {
            builder
                .ToTable("Assets");

            builder
                .HasKey(asset => asset.AssetId);

            builder
                .Property(asset => asset.AssetName)
                .IsRequired()
                .HasColumnType("varchar")
                .HasMaxLength(Asset.MAX_ASSETNAME_LENGTH);

            builder.Property(d => d.Balance)
                .HasColumnType("decimal(18,2)");

            builder
                .HasOne(asset => asset.Account)
                .WithMany(account => account.Assets)
                .HasForeignKey(asset => asset.AccountId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
