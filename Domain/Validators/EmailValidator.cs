using Domain.ValueObjects;
using FluentValidation;
using System.Net.Mail;

namespace Domain.Validators
{
    public class EmailValidator : AbstractValidator<Email>
    {
        public EmailValidator()
        {
            RuleFor(x => x.GetValue())
                .NotEmpty()
                .WithMessage("Email cannot be null, empty or whitespace");

            RuleFor(X => X.GetValue())
                .Must(ValidateEmailMatch)
                .WithMessage("Email does not match pattern");

            var maxCharacters = 50;
            RuleFor(x => x.GetValue())
                .MaximumLength(maxCharacters)
                .WithMessage($"Email cannot be longer than {maxCharacters} characters");
        }

        private bool ValidateEmailMatch(string email)
        {
            try
            {
                new MailAddress(email);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
