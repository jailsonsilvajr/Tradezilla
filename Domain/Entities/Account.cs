using Domain.Exceptions;
using Domain.Validators;

namespace Domain.Entities
{
    public class Account
    {
        private static readonly AccountValidator _validator = new AccountValidator();
        public Guid AccountId { get; }
        public string Name { get; }
        public string Email { get; }
        public string Document { get; }
        public string Password { get; }

        private Account(Guid accountId, string name, string email, string document, string password)
        {
            AccountId = accountId;
            Name = name;
            Email = email;
            Document = document;
            Password = password;
        }

        public static Account Create(string name, string email, string document, string password)
        {
            var newAccount = new Account(Guid.NewGuid(), name, email, document, password);
            var validationResult = _validator.Validate(newAccount);
            if (!validationResult.IsValid)
            {
                throw new ValidationException("", validationResult.Errors);
            }

            return newAccount;
        }
    }
}
