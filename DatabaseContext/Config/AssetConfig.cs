using DatabaseContext.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DatabaseContext.Config
{
    public class AssetConfig : IEntityTypeConfiguration<AssetModel>
    {
        public void Configure(EntityTypeBuilder<AssetModel> builder)
        {
            builder
                .ToTable("Assets");

            builder
                .HasKey(asset => asset.AssetId);

            builder
                .Property(asset => asset.AssetName)
                .IsRequired()
                .HasColumnType("varchar")
                .HasMaxLength(5);

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
