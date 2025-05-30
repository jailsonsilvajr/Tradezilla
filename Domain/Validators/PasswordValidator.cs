using Domain.ValueObjects;
using FluentValidation;
using System.Text.RegularExpressions;

namespace Domain.Validators
{
    public class PasswordValidator : AbstractValidator<Password>
    {
        public PasswordValidator()
        {
            RuleFor(x => x.GetValue())
                .NotEmpty()
                .WithMessage("Password cannot be null, empty ou whitespace");

            var maxCharacters = 14;
            RuleFor(x => x.GetValue())
                .MaximumLength(maxCharacters)
                .WithMessage($"Password cannot be longer than {maxCharacters} characters");

            RuleFor(x => x.GetValue())
                .Must(password =>
                    password is not null
                    && Regex.IsMatch(password, @"\d+")
                    && Regex.IsMatch(password, @"[a-z]+")
                    && Regex.IsMatch(password, @"[A-Z]+"))
                .WithMessage("Password does not match pattern");
        }
    }
}
