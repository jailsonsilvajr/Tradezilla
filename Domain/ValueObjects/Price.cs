using Domain.Exceptions;
using Domain.Validators;

namespace Domain.ValueObjects
{
    public class Price
    {
        private readonly PriceValidator _validator = new PriceValidator();
        private readonly decimal _value;

        public Price(decimal value)
        {
            _value = value;

            var validationResult = _validator.Validate(this);
            if (!validationResult.IsValid)
            {
                throw new ValidationException("Invalid price", validationResult.Errors);
            }
        }

        public decimal GetValue() => _value;
    }
}
