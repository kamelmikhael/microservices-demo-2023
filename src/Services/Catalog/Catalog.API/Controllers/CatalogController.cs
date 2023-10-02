using Catalog.API.Entities;
using Catalog.API.Repositories;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var result = await _productRepository.GetProductsAsync();
            return Ok(result);
        }


        [HttpGet("[action]/{category}", Name = nameof(GetProductByCategory))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Product>))]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductByCategory(string category)
        {
            var result = await _productRepository.GetProductsByCategoryAsync(category);
            return Ok(result);
        }

        [HttpGet("[action]/{name}", Name = nameof(GetProductByName))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Product>))]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductByName(string name)
        {
            var products = await _productRepository.GetProductsByNameAsync(name);

            return Ok(products);
        }

        [HttpGet("{id:length(24)}", Name = nameof(GetProductById))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Product))]
        public async Task<ActionResult<Product>> GetProductById(string id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);

            if(product is null)
            {
                _logger.LogError($"Product with id: {id}, Not Found.");
                return NotFound();
            }

            return Ok(product);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Product))]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
        {
            await _productRepository.CreateProductAsync(product);
            return CreatedAtRoute("GetProductById", new { Id = product.Id }, product);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Product))]
        public async Task<ActionResult<Product>> UpdateProduct([FromBody] Product product)
        {
            await _productRepository.UpdateProductAsync(product);
            return Ok(product);
        }

        [HttpDelete("{id:length(24)}", Name = nameof(DeleteProduct))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Product))]
        public async Task<ActionResult<Product>> DeleteProduct(string id)
        {
            await _productRepository.DeleteProductAsync(id);
            return Ok();
        }
    }
}