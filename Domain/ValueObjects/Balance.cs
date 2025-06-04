using Domain.Exceptions;
using Domain.Validators;

namespace Domain.ValueObjects
{
    public class Balance
    {
        private readonly BalanceValidator _validator = new BalanceValidator();
        private readonly decimal _value;

        public Balance(decimal value)
        {
            _value = value;

            var validationResult = _validator.Validate(this);
            if (!validationResult.IsValid)
            {
                throw new ValidationException("Invalid balance value", validationResult.Errors);
            }
        }

        public decimal GetValue() => _value;
    }
}
