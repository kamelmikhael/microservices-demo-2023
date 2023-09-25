using Catalog.API.Data;
using Catalog.API.Entities;
using MongoDB.Driver;
using System.Xml.Linq;

namespace Catalog.API.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ICatalogContext _context;

    public ProductRepository(ICatalogContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        return await _context
                        .Products
                        .Find(x => true)
                        .ToListAsync();
    }

    public async Task<Product> GetProductByIdAsync(string id)
    {
        return await _context
                        .Products
                        .Find(x => x.Id == id)
                        .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(string category)
    {
        FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Category, category);
        return await GetProductsByFilterAsync(filter);
    }

    public async Task<IEnumerable<Product>> GetProductsByNameAsync(string name)
    {
        FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Name, name);
        return await GetProductsByFilterAsync(filter);
    }

    private async Task<IEnumerable<Product>> GetProductsByFilterAsync(FilterDefinition<Product> filter)
    {
        return await _context
                        .Products
                        .Find(filter)
                        .ToListAsync();
    }

    public async Task CreateProductAsync(Product product)
    {
        await _context.Products.InsertOneAsync(product);
    }

    public async Task<bool> UpdateProductAsync(Product product)
    {
        var updateResult =  await _context
                    .Products
                    .ReplaceOneAsync(filter: p => p.Id == product.Id,replacement: product);

        return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
    }

    public async Task<bool> DeleteProductAsync(string id)
    {
        FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Id, id);
        var deleteResult = await _context
                    .Products
                    .DeleteOneAsync(filter);

        return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
    }
}
