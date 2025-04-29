namespace API.DTOs
{
    public class AccountDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Document { get; set; }
        public required string Password { get; set; }
    }
}
