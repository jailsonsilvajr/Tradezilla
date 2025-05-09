using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DatabaseContext.Config
{
    public class DepositConfig : IEntityTypeConfiguration<Deposit>
    {
        public void Configure(EntityTypeBuilder<Deposit> builder)
        {
            builder.ToTable("Deposits");
            
            builder.HasKey(d => d.DepositId);

            builder.Property(d => d.Quantity)
                .HasColumnType("decimal(18,2)");

            builder
                .HasOne(d => d.Asset)
                .WithMany(a => a.Deposits)
                .HasForeignKey(d => d.AssetId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
