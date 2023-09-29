using MediatR;

namespace Ordering.Application.Features.Orders.Queries.List;

public class OrderGetListQuery : IRequest<List<OrderListResponse>>
{
    public string UserName { get; private set; }

    public OrderGetListQuery(string userName)
    {
        UserName = userName ?? throw new ArgumentNullException(nameof(userName));
    }

}
