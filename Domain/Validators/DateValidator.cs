using Domain.ValueObjects;
using FluentValidation;

namespace Domain.Validators
{
    public class DateValidator : AbstractValidator<Date>
    {
        public DateValidator()
        {
            RuleFor(date => date.GetValue())
                .GreaterThan(DateTime.MinValue)
                .WithMessage($"Date must be greater than {DateTime.MinValue}");
        }
    }
}
