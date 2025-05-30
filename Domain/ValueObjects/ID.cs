using Domain.Exceptions;
using Domain.Validators;

namespace Domain.ValueObjects
{
    public class ID
    {
        private static readonly IDValidator _validator = new IDValidator();
        private readonly Guid _value;

        public ID(Guid value)
        {
            _value = value;

            var validationResult = _validator.Validate(this);
            if (!validationResult.IsValid)
            {
                throw new ValidationException("ID cannot be created", validationResult.Errors);
            }
        }

        public Guid GetValue() => _value;
    }
}
