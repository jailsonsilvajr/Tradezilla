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

            builder
                .Property(d => d.AssetId)
                .HasColumnType("varchar")
                .HasMaxLength(Deposit.MAX_ASSETID_LENGTH);

            builder.Property(d => d.Quantity)
                .HasColumnType("decimal(18,2)");

            builder
                .HasOne(d => d.Account)
                .WithMany(a => a.Deposits)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
