namespace Domain.Entities
{
    public class Account
    {
        public Account(Guid accountId, string name, string email, string document, string password)
        {
            AccountId = accountId;
            Name = name;
            Email = email;
            Document = document;
            Password = password;
        }

        public Guid AccountId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Document { get; set; }
        public string Password { get; set; }
    }
}
