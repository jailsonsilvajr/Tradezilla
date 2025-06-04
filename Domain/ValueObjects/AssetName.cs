using Domain.Exceptions;
using Domain.Validators;

namespace Domain.ValueObjects
{
    public class AssetName
    {
        private readonly AssetNameValidator _validator = new AssetNameValidator();
        private readonly string _value;

        public AssetName(string assetName)
        {
            _value = assetName;
            var validationResult = _validator.Validate(this);
            if (!validationResult.IsValid)
            {
                throw new ValidationException("Invalid asset name", validationResult.Errors);
            }
        }

        public string GetValue() => _value;
    }
}
