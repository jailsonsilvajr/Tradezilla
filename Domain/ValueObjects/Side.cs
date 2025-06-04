using Domain.Exceptions;
using Domain.Validators;

namespace Domain.ValueObjects
{
    public class Side
    {
        private readonly SideValidator _validator = new SideValidator();
        private readonly string _value;

        public Side(string side)
        {
            _value = side;

            var validationResult = _validator.Validate(this);
            if (!validationResult.IsValid)
            {
                throw new ValidationException("Invalid side", validationResult.Errors);
            }
        }

        public string GetValue() => _value;
    }
}
