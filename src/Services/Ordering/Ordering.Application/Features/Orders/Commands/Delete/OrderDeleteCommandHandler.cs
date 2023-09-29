using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Exceptions;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.Orders.Commands.Delete;

public class OrderDeleteCommandHandler : IRequestHandler<OrderDeleteCommand>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ILogger<OrderDeleteCommandHandler> _logger;

    public OrderDeleteCommandHandler(
        IOrderRepository orderRepository,
        ILogger<OrderDeleteCommandHandler> logger)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Unit> Handle(OrderDeleteCommand request, CancellationToken cancellationToken)
    {
        var orderToDelete = await _orderRepository.GetByIdAsync(request.Id) 
            ?? throw new NotFoundException(nameof(Order), request.Id);

        await _orderRepository.DeleteAsync(orderToDelete);

        _logger.LogInformation($"Order with Id: {request.Id} is successfully deleted.");

        return Unit.Value;
    }
}