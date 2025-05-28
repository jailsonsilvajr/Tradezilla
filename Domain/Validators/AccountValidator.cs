using Domain.Entities;
using FluentValidation;
using System.Text.RegularExpressions;

namespace Domain.Validators
{
    public class AccountValidator : AbstractValidator<Account>
    {
        public AccountValidator()
        {
            RuleFor(x => x.Password)
                .Must(password => 
                    !string.IsNullOrEmpty(password)
                    && Regex.IsMatch(password, @"\d+")
                    && Regex.IsMatch(password, @"[a-z]+")
                    && Regex.IsMatch(password, @"[A-Z]+")
                    && password.Length <= Account.MAX_PASSWORD_LENGTH)
                .WithMessage("Invalid password");
        }
    }
}
