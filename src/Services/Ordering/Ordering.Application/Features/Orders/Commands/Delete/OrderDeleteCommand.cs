using MediatR;

namespace Ordering.Application.Features.Orders.Commands.Delete;

public class OrderDeleteCommand : IRequest
{
    public int Id { get; }

    public OrderDeleteCommand(int id)
    {
        Id = id;
    }
}
