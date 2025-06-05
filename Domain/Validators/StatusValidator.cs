using Domain.ValueObjects;
using FluentValidation;

namespace Domain.Validators
{
    public class StatusValidator : AbstractValidator<Status>
    {
        public StatusValidator()
        {
            RuleFor(status => status.GetValue())
                .Must(x => !string.IsNullOrWhiteSpace(x) && (x.Equals("open") || x.Equals("closed")))
                .WithMessage("Status must be either 'open' or 'closed'");
        }
    }
}
