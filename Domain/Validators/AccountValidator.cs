using Domain.Entities;
using FluentValidation;
using System.Text.RegularExpressions;

namespace Domain.Validators
{
    public class AccountValidator : AbstractValidator<Account>
    {
        public AccountValidator()
        {
            RuleFor(x => x.Name)
                .Must(name => 
                    !string.IsNullOrEmpty(name) 
                    && Regex.IsMatch(name, @"[a-zA-Z] [a-zA-Z]+") 
                    && name.Length <= Account.MAX_NAME_LENGTH)
                .WithMessage("Invalid name");
            RuleFor(x => x.Email)
                .Must(email => 
                    !string.IsNullOrEmpty(email) 
                    && Regex.IsMatch(email, @"^(.+)\@(.+)$") 
                    && email.Length <= Account.MAX_EMAIL_LENGTH)
                .WithMessage("Invalid email");

            RuleFor(x => x.Document)
                .Must(ValidateDocument)
                .WithMessage("Invalid document");

            RuleFor(x => x.Password)
                .Must(password => 
                    !string.IsNullOrEmpty(password)
                    && Regex.IsMatch(password, @"\d+")
                    && Regex.IsMatch(password, @"[a-z]+")
                    && Regex.IsMatch(password, @"[A-Z]+")
                    && password.Length <= Account.MAX_PASSWORD_LENGTH)
                .WithMessage("Invalid password");
        }

        private static bool ValidateDocument(string? document)
        {
            if (document is null)
            {
                return false;
            }

            document = Account.CleanDocument(document);

            if (document is null || document.Length != Account.MAX_DOCUMENT_LENGTH)
            {
                return false;
            }

            if (AllDigitsEqual(document))
            {
                return false;
            }

            var digit1 = CalculateDigit(document, 10);
            var digit2 = CalculateDigit(document, 11);
            return ExtractDigit(document).Equals($"{digit1}{digit2}");
        }

        private static bool AllDigitsEqual(string document)
        {
            var firstDigit = document[0];
            return document.All(digit => digit.Equals(firstDigit));
        }

        private static int CalculateDigit(string document, int factor)
        {
            var total = 0;
            foreach (var digit in document)
            {
                if (factor > 1)
                {
                    total += (digit - '0') * factor--;
                }
            }

            var rest = total % 11;
            return (rest < 2) ? 0 : 11 - rest;
        }

        private static string ExtractDigit(string document)
        {
            return document.Substring(document.Length - 2, 2);
        }
    }
}
