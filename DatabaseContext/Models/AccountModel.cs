namespace DatabaseContext.Models
{
    public class AccountModel
    {
        public Guid Id { get; }
        public string? FullName { get; set; }
        public string? EmailAdrress { get; set; }
        public string? Document { get; set; }
        public string? Password { get; set; }
    }
}
