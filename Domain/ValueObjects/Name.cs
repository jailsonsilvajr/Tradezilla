using Domain.Exceptions;
using Domain.Validators;

namespace Domain.ValueObjects
{
    public class Name
    {
        private static readonly NameValidator _validator = new NameValidator();
        private string? _value;

        public Name(string? name)
        {
            _value = name;

            var validationResult = _validator.Validate(this);
            if (!validationResult.IsValid)
            {
                throw new ValidationException("Name cannot be created", validationResult.Errors);
            }
        }

        public string GetValue() => _value!;
    }
}
