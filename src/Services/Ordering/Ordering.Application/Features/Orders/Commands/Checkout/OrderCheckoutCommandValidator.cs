using FluentValidation;

namespace Ordering.Application.Features.Orders.Commands.Checkout;

public class OrderCheckoutCommandValidator : AbstractValidator<OrderCheckoutCommand>
{
    public OrderCheckoutCommandValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty()
            .NotNull()
            .MaximumLength(50);

        RuleFor(x => x.EmailAddress)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.TotalPrice)
            .NotEmpty()
            .GreaterThan(0);
    }
}