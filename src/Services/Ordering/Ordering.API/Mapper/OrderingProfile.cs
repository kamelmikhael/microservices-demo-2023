using AutoMapper;
using EventBus.Messages.Events;
using Ordering.Application.Features.Orders.Commands.Checkout;

namespace Ordering.API.Mapper;

public class OrderingProfile : Profile
{
    public OrderingProfile()
    {
        CreateMap<BasketCheckoutEvent, OrderCheckoutCommand>().ReverseMap();
    }
}
