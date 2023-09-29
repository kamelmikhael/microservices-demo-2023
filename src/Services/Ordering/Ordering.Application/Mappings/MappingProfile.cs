using AutoMapper;
using Ordering.Application.Features.Orders.Commands.Checkout;
using Ordering.Application.Features.Orders.Commands.Update;
using Ordering.Application.Features.Orders.Queries.List;
using Ordering.Domain.Entities;

namespace Ordering.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Order, OrderListResponse>();
        CreateMap<OrderCheckoutCommand, Order>();
        CreateMap<Order, OrderUpdateCommand>();
    }
}
