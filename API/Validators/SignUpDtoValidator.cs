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

            RuleFor(x => x.Email)
                .Must(email => !string.IsNullOrEmpty(email) && Regex.IsMatch(email, @"^(.+)\@(.+)$"))
                .WithMessage("Invalid email");

            RuleFor(x => x.Document)
                .Must(ValidateDocument)
                .WithMessage("Invalid document");
        }

        private bool ValidateDocument(string? document)
        {
            const int VALID_LENGTH = 11;

            if (document is null)
            {
                return false;
            }

            document = CleanDocument(document);

            if (document.Length != VALID_LENGTH)
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

        private string CleanDocument(string document)
        {
            return document.Trim()
                .Replace(".", "")
                .Replace("-", "");  
        }

        private bool AllDigitsEqual(string document)
        {
            var firstDigit = document[0];
            return document.All(digit => digit.Equals(firstDigit));
        }

        private int CalculateDigit(string document, int factor)
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

        private string ExtractDigit(string document)
        {
            return document.Substring(document.Length - 2, 2);
        }
    }
}
