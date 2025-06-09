using Domain.ValueObjects;

namespace Domain.Entities
{
    public class Account
    {
        private readonly ID _id;
        private readonly Name _name;
        private readonly Email _email;
        private readonly Document _document;
        private readonly Password _password;

        public Account(Guid accountId, string? name, string? email, string document, string password)
        {
            _id = new ID(accountId);
            _name = new Name(name);
            _email = new Email(email);
            _document = new Document(document);
            _password = new Password(password);
        }

        public Guid GetId() => _id.GetValue();
        public string GetName() => _name.GetValue();
        public string GetEmail() => _email.GetValue();
        public string GetDocument() => _document.GetValue();
        public string GetPassword() => _password.GetValue();

        public static Account Create(string? name, string? email, string document, string password)
        {
            var newAccount = new Account(Guid.NewGuid(), name, email, document, password);
            return newAccount;
        }
    }
}
