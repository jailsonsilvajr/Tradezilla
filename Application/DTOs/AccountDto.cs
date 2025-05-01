using Domain.Entities;

namespace Application.DTOs
{
    public class AccountDto
    {
        public static readonly int MAX_NAME_LENGTH = Account.MAX_NAME_LENGTH;
        public static readonly int MAX_EMAIL_LENGTH = Account.MAX_EMAIL_LENGTH;
        public static readonly int MAX_DOCUMENT_LENGTH = Account.MAX_DOCUMENT_LENGTH;
        public static readonly int MAX_PASSWORD_LENGTH = Account.MAX_PASSWORD_LENGTH;
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Document { get; set; }
        public string? Password { get; set; }
    }
}
