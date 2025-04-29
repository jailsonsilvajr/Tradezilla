namespace API.DTOs
{
    public class SignUpDto
    {
        public string? Name { get; set; }
        public required string Email { get; set; }
        public required string Document { get; set; }
        public required string Password { get; set; }
    }
}
