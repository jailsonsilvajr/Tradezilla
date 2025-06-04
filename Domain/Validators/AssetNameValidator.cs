using Domain.ValueObjects;
using FluentValidation;

namespace Domain.Validators
{
    public class AssetNameValidator : AbstractValidator<AssetName>
    {
        public AssetNameValidator()
        {
            RuleFor(asset => asset.GetValue())
                .NotEmpty()
                .WithMessage("AssetName cannot be null, empty or whitespace");

            var assetNameMaxLength = 5;
            RuleFor(asset => asset.GetValue())
                .Must(x =>
                    !string.IsNullOrEmpty(x)
                    && x.Length > 0 && x.Length <= assetNameMaxLength)
                .WithMessage($"AssetName must be between 1 and {assetNameMaxLength} characters long");
        }
    }
}
