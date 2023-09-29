using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Models;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.Orders.Commands.Checkout;

public class OrderCheckoutCommandHandler : IRequestHandler<OrderCheckoutCommand, int>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;
    private readonly IEmailService _emailService;
    private readonly ILogger<OrderCheckoutCommandHandler> _logger;

    public OrderCheckoutCommandHandler(
        IOrderRepository orderRepository,
        IMapper mapper,
        IEmailService emailService,
        ILogger<OrderCheckoutCommandHandler> logger)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<int> Handle(OrderCheckoutCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Order>(request);

        var newEntity = await _orderRepository.AddAsync(entity);

        await SendEmail(newEntity);

        return newEntity.Id;
    }

    private async Task SendEmail(Order order)
    {
        var email = new Email()
        {
            To = "test@gmail.com",
            Body = $"Order was created with Id: {order.Id}",
            Subject = "New Order Created"
        };

        try
        {
            await _emailService.SendAsync(email);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error on sending email with exception: {ex.Message}");
        }
    }
}
