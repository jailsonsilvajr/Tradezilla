using Domain.Entities;
using FluentValidation;

namespace Domain.Validators
{
    public class OrderValidator : AbstractValidator<Order>
    {
        public OrderValidator()
        {
            RuleFor(order => order.AccountId)
                .NotEmpty()
                .WithMessage("AccountId cannot be empty");

            RuleFor(order => order.Market)
                .NotEmpty()
                .WithMessage("Market cannot be null, empty or whitespace")
                .Length(1, Order.MAX_MARKET_LENGTH)
                .WithMessage($"Market must be between 1 and {Order.MAX_MARKET_LENGTH} characters long");

            RuleFor(order => order.Side)
                .NotEmpty()
                .WithMessage("Side cannot be null, empty or whitespace")
                .Length(1, Order.MAX_SIDE_LENGTH)
                .WithMessage($"Side must be between 1 and {Order.MAX_SIDE_LENGTH} characters long");

            RuleFor(order => order.Quantity)
                .GreaterThan(0)
                .WithMessage("Quantity must be greater than 0");

            RuleFor(order => order.Price)
                .GreaterThan(0)
                .WithMessage("Price must be greater than 0");

            RuleFor(order => order.CreatedAt)
                .NotNull()
                .WithMessage("CreatedAt cannot be null");
        }
    }
}
