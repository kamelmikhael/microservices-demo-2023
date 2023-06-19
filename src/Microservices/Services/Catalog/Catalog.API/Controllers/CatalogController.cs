using Catalog.API.Entities;
using Catalog.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CatalogController : ControllerBase
    {
        private readonly ILogger<CatalogController> _logger;
        private readonly IProductRepository _productRepository;

        public CatalogController(
            ILogger<CatalogController> logger, 
            IProductRepository productRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Product>))]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsAsync()
        {
            var result = await _productRepository.GetAllProductsAsync();
            return Ok(result);
        }


        [HttpGet("[action]/{category}", Name = "GetProductsByCategory")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Product>))]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByCategoryAsync(string category)
        {
            var result = await _productRepository.GetProductsByCategoryAsync(category);
            return Ok(result);
        }

        [HttpGet("{id:length(24)}", Name = "GetProduct")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Product))]
        public async Task<ActionResult<Product>> GetProductByIdAsync(string id)
        {
            var result = await _productRepository.GetProductByIdAsync(id);

            if(result is null)
            {
                _logger.LogError($"Product with id: {id}, Not Found.");
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Product))]
        public async Task<ActionResult<Product>> CreateProductAsync([FromBody] Product product)
        {
            await _productRepository.CreateProductAsync(product);
            return CreatedAtRoute("GetProduct", new { Id = product.Id }, product);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Product))]
        public async Task<ActionResult<Product>> UpdateProductAsync([FromBody] Product product)
        {
            await _productRepository.UpdateProductAsync(product);
            return Ok(product);
        }

        [HttpDelete("{id:length(24)}", Name = "DeleteProduct")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Product))]
        public async Task<ActionResult<Product>> DeleteProductAsync(string id)
        {
            await _productRepository.DeleteProductAsync(id);
            return Ok();
        }
    }
}