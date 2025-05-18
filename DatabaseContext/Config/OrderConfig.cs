using DatabaseContext.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DatabaseContext.Config
{
    public class OrderConfig : IEntityTypeConfiguration<OrderModel>
    {
        public void Configure(EntityTypeBuilder<OrderModel> builder)
        {
            builder.ToTable("Orders");
            builder.HasKey(order => order.OrderId);

            builder.Property(order => order.Market)
                .HasColumnType("varchar")
                .HasMaxLength(7)
                .IsRequired();

            builder.Property(order => order.Side)
                .HasColumnType("varchar")
                .HasMaxLength(5)
                .IsRequired();

            builder.Property(order => order.Quantity)
                .IsRequired();

            builder.Property(order => order.Price)
                .HasColumnType("decimal(19,4)");

            builder.Property(order => order.FillQuantity)
                .IsRequired();

            builder.Property(order => order.FillPrice)
                .HasColumnType("decimal(19,4)");

            builder.Property(order => order.CreatedAt)
                .IsRequired();

            builder.Property(order => order.Status)
                .HasColumnType("varchar")
                .HasMaxLength(10)
                .IsRequired();

            builder
                .HasOne(order => order.Account)
                .WithMany(account => account.Orders)
                .HasForeignKey(order => order.AccountId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
