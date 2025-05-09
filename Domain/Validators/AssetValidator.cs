using Domain.Entities;
using FluentValidation;

namespace Domain.Validators
{
    public class AssetValidator : AbstractValidator<Asset>
    {
        public AssetValidator()
        {
            RuleFor(asset => asset.AccountId)
                .NotEmpty()
                .WithMessage("AccountId cannot be empty");

            RuleFor(asset => asset.AssetName)
                .Must(x =>
                    !string.IsNullOrEmpty(x)
                    && x.Length > 0 && x.Length <= Asset.MAX_ASSETNAME_LENGTH)
                .WithMessage($"AssetName must be between 1 and {Asset.MAX_ASSETNAME_LENGTH} characters long");

            RuleFor(asset => asset.Balance)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Balance must be greater than or equal to 0");
        }
    }
}
