using Domain.Exceptions;
using Domain.Validators;

namespace Domain.ValueObjects
{
    public class Status
    {
        private readonly StatusValidator _validator = new StatusValidator();
        private readonly string _value;

        public Status(string status)
        {
            _value = status;

            var validationResult = _validator.Validate(this);
            if (!validationResult.IsValid)
            {
                throw new ValidationException("Invalid status", validationResult.Errors);
            }
        }

        public string GetValue() => _value;
    }
}
