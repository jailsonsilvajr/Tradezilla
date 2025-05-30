using Domain.Exceptions;
using Domain.Validators;

namespace Domain.ValueObjects
{
    public class Document
    {
        private readonly DocumentValidator _validator = new DocumentValidator();
        private readonly string _value;

        public Document(string document)
        {
            _value = document;

            var validationResult = _validator.Validate(this);
            if (!validationResult.IsValid)
            {
                throw new ValidationException("Invalid document", validationResult.Errors);
            }
        }

        public string GetValue() => _value;
    }
}
