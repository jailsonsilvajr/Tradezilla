using Domain.Exceptions;
using Domain.Validators;

namespace Domain.ValueObjects
{
    public class Password
    {
        private readonly PasswordValidator _validator = new PasswordValidator();
        private readonly string _value;
        
        public Password(string password)
        {
            _value = password;

            var validationResult = _validator.Validate(this);
            if (!validationResult.IsValid)
            {
                throw new ValidationException("Invalid password", validationResult.Errors);
            }
        }

        public string GetValue() => _value;
    }
}
