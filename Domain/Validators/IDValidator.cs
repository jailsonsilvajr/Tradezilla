using Domain.ValueObjects;
using FluentValidation;

namespace Domain.Validators
{
    public class IDValidator : AbstractValidator<ID>
    {
        public IDValidator()
        {
            RuleFor(x => x.GetValue())
                .NotEmpty()
                .WithMessage("ID cannot be null or empty");
        }
    }
}
