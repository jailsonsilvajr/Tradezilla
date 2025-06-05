using Domain.Exceptions;
using Domain.Validators;

namespace Domain.ValueObjects
{
    public class Date
    {
        private readonly DateValidator _validator = new DateValidator();
        private readonly DateTime _value;

        public Date(DateTime value)
        {
            _value = value;

            var validationResult = _validator.Validate(this);
            if (!validationResult.IsValid)
            {
                throw new ValidationException("Invalid date", validationResult.Errors);
            }
        }

        public DateTime GetValue() => _value;
    }
}
