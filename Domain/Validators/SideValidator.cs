using Domain.ValueObjects;
using FluentValidation;

namespace Domain.Validators
{
    public class SideValidator : AbstractValidator<Side>
    {
        public SideValidator()
        {
            var sideMaxLength = 5;
            RuleFor(x => x.GetValue())
                .NotEmpty()
                .WithMessage("Side cannot be null, empty or whitespace")
                .Length(1, sideMaxLength)
                .WithMessage($"Side must be between 1 and {sideMaxLength} characters long");
        }
    }
}
