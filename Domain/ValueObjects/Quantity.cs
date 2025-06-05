using Domain.Exceptions;
using Domain.Validators;

namespace Domain.ValueObjects
{
    public class Quantity
    {
        private readonly QuantityValidator _validator = new QuantityValidator();
        private readonly int _value;

        public Quantity(int quantity)
        {
            _value = quantity;

            var validationResult = _validator.Validate(this);
            if (!validationResult.IsValid)
            {
                throw new ValidationException("Invalid quantity", validationResult.Errors);
            }
        }

        public int GetValue() => _value;
    }
}
