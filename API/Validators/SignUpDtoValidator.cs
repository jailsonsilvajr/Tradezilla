using API.DTOs;
using FluentValidation;
using System.Text.RegularExpressions;

namespace API.Validators
{
    public class SignUpDtoValidator : AbstractValidator<SignUpDto>
    {
        public SignUpDtoValidator()
        {
            RuleFor(x => x.Name)
                .Must(name => !string.IsNullOrEmpty(name) && Regex.IsMatch(name, @"[a-zA-Z] [a-zA-Z]+"))
                .WithMessage("Invalid name");
        }
    }
}
