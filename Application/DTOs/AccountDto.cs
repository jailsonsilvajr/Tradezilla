namespace Application.DTOs
{
    public class AccountDto
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Document { get; set; }
        public List<AssetDto>? Assets { get; set; }
    }
}
