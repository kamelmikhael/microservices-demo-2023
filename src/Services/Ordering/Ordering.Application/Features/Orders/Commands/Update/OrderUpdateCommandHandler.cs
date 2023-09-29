using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Exceptions;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.Orders.Commands.Update;

public class OrderUpdateCommandHandler : IRequestHandler<OrderUpdateCommand>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<OrderUpdateCommandHandler> _logger;

    public OrderUpdateCommandHandler(
        IOrderRepository orderRepository,
        IMapper mapper,
        ILogger<OrderUpdateCommandHandler> logger)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Unit> Handle(OrderUpdateCommand request, CancellationToken cancellationToken)
    {
        var orderToEdit = await _orderRepository.GetByIdAsync(request.Id)
            ?? throw new NotFoundException(nameof(Order), request.Id);

        _mapper.Map(request, orderToEdit, typeof(OrderUpdateCommand), typeof(Order));

        await _orderRepository.UpdateAsync(orderToEdit);

        _logger.LogInformation($"Order with Id: {request.Id} updated successfully");

        return Unit.Value;
    }
}
