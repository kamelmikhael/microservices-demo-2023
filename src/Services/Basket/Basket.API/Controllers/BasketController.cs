using AutoMapper;
using Basket.API.Entities;
using Basket.API.GrpcServices;
using Basket.API.Repositories;
using EventBus.Messages.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly ILogger<BasketController> _logger;
        private readonly IBasketRepository _basketRepository;
        private readonly DiscountGrpcService _discountGrpcService;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;

        public BasketController(
            ILogger<BasketController> logger,
            IBasketRepository basketRepository,
            DiscountGrpcService discountGrpcService,
            IMapper mapper,
            IPublishEndpoint publishEndpoint)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _basketRepository = basketRepository ?? throw new ArgumentNullException(nameof(basketRepository));
            _discountGrpcService = discountGrpcService ?? throw new ArgumentNullException(nameof(discountGrpcService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
        }

        [HttpGet("{userName}", Name = nameof(GetBasket))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ShoppingCart))]
        public async Task<ActionResult<ShoppingCart>> GetBasket(string userName)
            => Ok(await _basketRepository.GetBasket(userName) ?? new ShoppingCart(userName));

        [HttpPost(Name = nameof(UpdateBasket))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ShoppingCart))]
        public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart basket)
        {
            //TODO: Communicate with Discount.Grpc
            foreach(var item in basket.Items)
            {
                var coupon = await _discountGrpcService.GetDiscountAsync(item.ProductName);

                item.Price -= coupon.Amount;
            }

            return Ok(await _basketRepository.UpdateBasket(basket));
        }

        [HttpDelete("{userName}", Name = nameof(DeleteBasket))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(void))]
        public async Task<ActionResult> DeleteBasket(string userName)
        {
            await _basketRepository.DeleteBasket(userName);

            return Ok();
        }

        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
        {
            // get existing basket with total price
            var basket = await _basketRepository.GetBasket(basketCheckout.UserName);
            if (basket == null)
            {
                return BadRequest();
            }

            // send checkout event to rabbitmq
            var eventMessage = _mapper.Map<BasketCheckoutEvent>(basketCheckout);
            eventMessage.TotalPrice = basket.TotalPrice;
            await _publishEndpoint.Publish<BasketCheckoutEvent>(eventMessage);

            // remove the basket
            await _basketRepository.DeleteBasket(basket.Username);

            return Accepted();
        }
    }
}