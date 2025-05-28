namespace Application.DTOs
{
    public class SignUpDto
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string Document { get; set; } = string.Empty;
        public string? Password { get; set; }
    }
}
