using AutoMapper;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Ordering.Application.Features.Orders.Commands.Checkout;

namespace Ordering.API.EventBusConsumers;

public class BasketcheckoutConsumer : IConsumer<BasketCheckoutEvent>
{
    private readonly IMapper _mapper;
    private readonly ILogger<BasketcheckoutConsumer> _logger;
    private readonly IMediator _mediator;

    public BasketcheckoutConsumer(
        IMapper mapper,
        ILogger<BasketcheckoutConsumer> logger,
        IMediator mediator)
    {
        _mapper = mapper;
        _logger = logger;
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
    {
        var command = _mapper.Map<OrderCheckoutCommand>(context.Message);

        await _mediator.Send(command);

        _logger.LogInformation("BasketCheckoutEvent consumed successflly.");
    }
}
