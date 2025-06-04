using Domain.Exceptions;
using Domain.Validators;

namespace Domain.ValueObjects
{
    public class Market
    {
        private readonly MarketValidator _validator = new MarketValidator();
        private readonly string _value;

        public Market(string market)
        {
            _value = market;

            var validationResult = _validator.Validate(this);
            if (!validationResult.IsValid)
            {
                throw new ValidationException("Invalid market", validationResult.Errors);
            }
        }

        public string GetValue() => _value;
    }
}
