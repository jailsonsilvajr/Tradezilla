using API.DTOs;
using FluentValidation;

namespace API.Validators
{
    public class SignUpDtoValidator : AbstractValidator<SignUpDto>
    {
        public SignUpDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required")
                .Matches(@"[a-zA-Z] [a-zA-Z]+")
                .WithMessage("Invalid name");
        }
    }
}
