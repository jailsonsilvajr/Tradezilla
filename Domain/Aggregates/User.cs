using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.Aggregates
{
    public class User
    {
        private readonly Account _account;
        private readonly Name _name;
        private readonly Email _email;
        private readonly Document _document;
        private readonly Password _password;

        public User(Guid accountId, string? name, string? email, string document, string password)
        {
            _account = new Account(accountId);
            _name = new Name(name);
            _email = new Email(email);
            _document = new Document(document);
            _password = new Password(password);
        }

        public Guid GetAccountId() => _account.GetId();
        public string GetName() => _name.GetValue();
        public string GetEmail() => _email.GetValue();
        public string GetDocument() => _document.GetValue();
        public string GetPassword() => _password.GetValue();

        public static User Create(string? name, string? email, string document, string password)
        {
            var newAccount = new User(Guid.NewGuid(), name, email, document, password);
            return newAccount;
        }
    }
}
