using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Features.Orders.Commands.Checkout;
using Ordering.Application.Features.Orders.Commands.Delete;
using Ordering.Application.Features.Orders.Commands.Update;
using Ordering.Application.Features.Orders.Queries.List;

namespace Ordering.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly ILogger<OrdersController> _logger;
        private readonly IMediator _mediator;

        public OrdersController(
            ILogger<OrdersController> logger, 
            IMediator mediator)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet("{userName}", Name = nameof(GetOrdersByUserName))]
        [ProducesResponseType(typeof(IEnumerable<OrderListResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<OrderListResponse>>> GetOrdersByUserName(string userName)
        {
            var query = new OrderGetListQuery(userName);
            var orders = await _mediator.Send(query);
            return Ok(orders);
        }

        //testing purpose
        [HttpPost(Name = nameof(CheckoutOrder))]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        public async Task<ActionResult<int>> CheckoutOrder([FromBody] OrderCheckoutCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut(Name = nameof(UpdateOrder))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<int>> UpdateOrder([FromBody] OrderUpdateCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPut("{id}", Name = nameof(DeleteOrder))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<int>> DeleteOrder(int id)
        {
            var command = new OrderDeleteCommand(id);
            await _mediator.Send(command);
            return NoContent();
        }
    }
}