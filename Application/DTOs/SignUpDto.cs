﻿namespace Application.DTOs
{
    public class SignUpDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Document { get; set; }
        public string? Password { get; set; }
    }
}
