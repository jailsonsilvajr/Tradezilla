using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DatabaseContext.Config
{
    public class AccountConfig : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ToTable("Accounts");

            builder.HasKey(x => x.AccountId);

            builder.Property(x => x.Name)
                .HasColumnType("varchar")
                .HasMaxLength(Account.MAX_NAME_LENGTH);

            builder.Property(x => x.Email)
                .HasColumnType("varchar")
                .HasMaxLength(Account.MAX_EMAIL_LENGTH);

            builder.Property(x => x.Document)
                .HasColumnType("varchar")
                .HasMaxLength(Account.MAX_DOCUMENT_LENGTH);

            builder.Property(x => x.Password)
                .HasColumnType("varchar")
                .HasMaxLength(Account.MAX_PASSWORD_LENGTH);
        }
    }
}
