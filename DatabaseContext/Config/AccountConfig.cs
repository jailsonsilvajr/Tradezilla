using Application.DTOs;
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

            builder.HasKey(x => x.Id);

            builder.Property(x => x.FullName)
                .HasColumnType("varchar")
                .HasMaxLength(AccountDto.MAX_NAME_LENGTH);

            builder.Property(x => x.EmailAdrress)
                .HasColumnType("varchar")
                .HasMaxLength(AccountDto.MAX_EMAIL_LENGTH);

            builder.Property(x => x.Document)
                .HasColumnType("varchar")
                .HasMaxLength(AccountDto.MAX_DOCUMENT_LENGTH);

            builder.Property(x => x.Password)
                .HasColumnType("varchar")
                .HasMaxLength(AccountDto.MAX_PASSWORD_LENGTH);
        }
    }
}
