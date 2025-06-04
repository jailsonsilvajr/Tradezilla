using Domain.Entities;
using FluentValidation;

namespace Domain.Validators
{
    public class OrderValidator : AbstractValidator<Order>
    {
        public OrderValidator()
        {
            RuleFor(order => order.Quantity)
                .GreaterThan(0)
                .WithMessage("Quantity must be greater than 0");

            RuleFor(order => order.Price)
                .GreaterThan(0)
                .WithMessage("Price must be greater than 0");

            RuleFor(order => order.CreatedAt)
                .NotNull()
                .WithMessage("CreatedAt cannot be null");

            RuleFor(order => order.Status)
                .NotEmpty()
                .WithMessage("Status cannot be null, empty or whitespace")
                .Length(1, Order.MAX_STATUS_LENGTH)
                .WithMessage($"Status must be between 1 and {Order.MAX_STATUS_LENGTH} characters long");
        }
    }
}
