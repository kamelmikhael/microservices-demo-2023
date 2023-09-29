using AutoMapper;
using MediatR;
using Ordering.Application.Contracts.Persistence;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.Orders.Queries.List;

public class OrderGetListQueryHandler : IRequestHandler<OrderGetListQuery, List<OrderListResponse>>
{
    private readonly IOrderRepository _repository;
    private readonly IMapper _mapper;

    public OrderGetListQueryHandler(
        IOrderRepository repository, 
        IMapper mapper)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<List<OrderListResponse>> Handle(
        OrderGetListQuery request,
        CancellationToken cancellationToken)
    {
        var entities = await _repository.GetOrdersByUserName(request.UserName);

        return _mapper.Map<List<OrderListResponse>>(entities);
    }
}
