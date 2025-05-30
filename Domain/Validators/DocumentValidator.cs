using Domain.ValueObjects;
using FluentValidation;

namespace Domain.Validators
{
    public class DocumentValidator : AbstractValidator<Document>
    {
        public DocumentValidator()
        {
            RuleFor(x => x.GetValue())
                .NotEmpty()
                .WithMessage("Document cannot be null, empty or whitespace");

            var maxCharacters = 11;
            RuleFor(x => x.GetValue())
                .Length(maxCharacters)
                .WithMessage($"Document must be {maxCharacters} characters long");

            RuleFor(x => x.GetValue())
                .Must(document => document is not null && ValidateDocument(document))
                .WithMessage("Document does not match pattern");
        }

        private static bool ValidateDocument(string document)
        {
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
