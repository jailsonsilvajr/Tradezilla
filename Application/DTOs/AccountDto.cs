namespace Application.DTOs
{
    public class AccountDto
    {
        public Guid AccountId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Document { get; set; }
        public string? Password { get; set; }
    }
}
