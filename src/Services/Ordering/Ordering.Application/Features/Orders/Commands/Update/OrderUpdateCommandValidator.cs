using FluentValidation;

namespace Ordering.Application.Features.Orders.Commands.Update;

public class OrderUpdateCommandValidator : AbstractValidator<OrderUpdateCommand>
{
    public OrderUpdateCommandValidator()
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