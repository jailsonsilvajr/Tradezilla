using Domain.Exceptions;
using Domain.Validators;

namespace Domain.Entities
{
    public class Account
    {
        public static readonly int MAX_NAME_LENGTH = 100;
        public static readonly int MAX_EMAIL_LENGTH = 50;
        public static readonly int MAX_DOCUMENT_LENGTH = 11;
        public static readonly int MAX_PASSWORD_LENGTH = 14;
        private static readonly AccountValidator _validator = new AccountValidator();

        public Guid AccountId { get; }
        public string? Name { get; }
        public string? Email { get; }
        public string? Document { get; }
        public string? Password { get; }
        public ICollection<Asset> Assets { get; }

        private Account(Guid accountId, string? name, string? email, string? document, string? password)
        {
            AccountId = accountId;
            Name = name;
            Email = email;
            Document = CleanDocument(document);
            Password = password;
            Assets = new List<Asset>();
        }

        public static Account Create(string? name, string? email, string? document, string? password)
        {
            var newAccount = new Account(Guid.NewGuid(), name, email, document, password);
            var validationResult = _validator.Validate(newAccount);
            if (!validationResult.IsValid)
            {
                throw new ValidationException("Invalid data to create account", validationResult.Errors);
            }

            return newAccount;
        }

        public static string? CleanDocument(string? document)
        {
            return document is null 
                ? default 
                : document.Trim().Replace(".", "").Replace("-", "");
        }
    }
}
