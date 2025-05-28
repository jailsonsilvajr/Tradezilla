using Domain.Exceptions;
using Domain.Validators;

namespace Domain.ValueObjects
{
    public class Email
    {
        private readonly EmailValidator _validator = new EmailValidator();
        private readonly string? _value;

        public Email(string? email)
        {
            _value = email;

            var validationResult = _validator.Validate(this);
            if (!validationResult.IsValid)
            {
                throw new ValidationException("Email cannot be created", validationResult.Errors);
            }
        }

        public string GetValue() => _value!;
    }
}
