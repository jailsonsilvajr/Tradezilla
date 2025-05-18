using DatabaseContext.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DatabaseContext.Config
{
    public class TransactionConfig : IEntityTypeConfiguration<TransactionModel>
    {
        public void Configure(EntityTypeBuilder<TransactionModel> builder)
        {
            builder.ToTable("Transactions");
            
            builder.HasKey(d => d.TransactionId);

            builder.Property(d => d.Quantity)
                .HasColumnType("decimal(18,2)");

            builder
                .HasOne(d => d.Asset)
                .WithMany(a => a.Transactions)
                .HasForeignKey(d => d.AssetId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
