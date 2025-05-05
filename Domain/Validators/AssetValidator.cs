using Domain.Entities;
using FluentValidation;

namespace Domain.Validators
{
    public class AssetValidator : AbstractValidator<Asset>
    {
        public AssetValidator()
        {
            RuleFor(deposit => deposit.AssetName)
                .Must(x =>
                    !string.IsNullOrEmpty(x)
                    && x.Length > 0 && x.Length <= Deposit.MAX_ASSETID_LENGTH)
                .WithMessage($"AssetName must be between 1 and {Asset.MAX_ASSETNAME_LENGTH} characters long");

            RuleFor(deposit => deposit.Balance)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Balance must be greater than or equal to 0");
        }
    }
}
