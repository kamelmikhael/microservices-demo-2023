using Discount.API.Entities;
using Discount.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Discount.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DiscountController : ControllerBase
    {
        private readonly ILogger<DiscountController> _logger;
        private readonly IDiscountReoistory _couponReoistory;

        public DiscountController(
            ILogger<DiscountController> logger, 
            IDiscountReoistory couponReoistory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _couponReoistory = couponReoistory ?? throw new ArgumentNullException(nameof(couponReoistory));
        }

        [HttpGet("{productName}", Name = nameof(GetDiscount))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Coupon))]
        public async Task<ActionResult<Coupon>> GetDiscount(string productName)
        {
            return Ok(await _couponReoistory.GetDiscount(productName));
        }

        [HttpPost(Name = nameof(CreateDiscount))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Coupon))]
        public async Task<ActionResult<Coupon>> CreateDiscount(Coupon coupon)
        {
            await _couponReoistory.CreateDiscount(coupon);
            return CreatedAtRoute(nameof(GetDiscount), new { productName = coupon.ProductName }, coupon);
        }

        [HttpPut(Name = nameof(UpdateDiscount))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        public async Task<ActionResult<bool>> UpdateDiscount(Coupon coupon)
        {
            return Ok(await _couponReoistory.UpdateDiscount(coupon));
        }

        [HttpDelete("{productName}", Name = nameof(DeleteDiscount))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        public async Task<ActionResult<bool>> DeleteDiscount(string productName)
        {
            return Ok(await _couponReoistory.DeleteDiscount(productName));
        }
    }
}