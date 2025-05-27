using Domain.ValueObjects;
using FluentValidation;
using System.Text.RegularExpressions;

namespace Domain.Validators
{
    public class NameValidator : AbstractValidator<Name>
    {
        public NameValidator()
        {
            RuleFor(x => x.GetValue())
                .NotEmpty()
                .WithMessage("Name cannot be null, empty or whitespace");

            RuleFor(x => x.GetValue())
                .Must(name => name is not null && Regex.IsMatch(name, @"[a-zA-Z] [a-zA-Z]+"))
                .WithMessage("Name does not match pattern");

            var maxCharacters = 100;
            RuleFor(x => x.GetValue())
                .MaximumLength(maxCharacters)
                .WithMessage($"Name cannot be longer than {maxCharacters} characters");
        }
    }
}
