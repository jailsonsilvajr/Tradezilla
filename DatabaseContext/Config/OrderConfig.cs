using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DatabaseContext.Config
{
    public class OrderConfig : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");
            builder.HasKey(order => order.OrderId);

            builder.Property(order => order.Market)
                .HasColumnType("varchar")
                .HasMaxLength(Order.MAX_MARKET_LENGTH)
                .IsRequired();

            builder.Property(order => order.Side)
                .HasColumnType("varchar")
                .HasMaxLength(Order.MAX_SIDE_LENGTH)
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
                .HasMaxLength(Order.MAX_STATUS_LENGTH)
                .IsRequired();

            builder
                .HasOne(order => order.Account)
                .WithMany(account => account.Orders)
                .HasForeignKey(order => order.AccountId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
