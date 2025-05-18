using DatabaseContext.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DatabaseContext.Config
{
    public class AccountConfig : IEntityTypeConfiguration<AccountModel>
    {
        public void Configure(EntityTypeBuilder<AccountModel> builder)
        {
            builder.ToTable("Accounts");

            builder.HasKey(x => x.AccountId);

            builder.Property(x => x.Name)
                .HasColumnType("varchar")
                .HasMaxLength(100);

            builder.Property(x => x.Email)
                .HasColumnType("varchar")
                .HasMaxLength(50);

            builder.Property(x => x.Document)
                .HasColumnType("varchar")
                .HasMaxLength(11);

            builder.Property(x => x.Password)
                .HasColumnType("varchar")
                .HasMaxLength(14);

            builder.HasIndex(x => x.Document)
                .IsUnique();
        }
    }
}
