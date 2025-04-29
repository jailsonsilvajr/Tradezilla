using API.DTOs;
using FluentValidation;

namespace API.Validators
{
    public class SignUpDtoValidator : AbstractValidator<SignUpDto>
    {
        public SignUpDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotNull()
                .WithMessage("Name can't be null")
                .Matches(@"[a-zA-Z] [a-zA-Z]+")
                .WithMessage("Invalid name");
        }
    }
}
