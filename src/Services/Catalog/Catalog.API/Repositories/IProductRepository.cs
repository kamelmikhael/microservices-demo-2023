using Catalog.API.Entities;

namespace Catalog.API.Repositories;

public interface IProductRepository
{
    #region Read Methods
    Task<IEnumerable<Product>> GetProductsAsync();

    Task<Product> GetProductByIdAsync(string id);

    Task<IEnumerable<Product>> GetProductsByNameAsync(string name);

    Task<IEnumerable<Product>> GetProductsByCategoryAsync(string category);
    #endregion

    #region Write Methods
    Task CreateProductAsync(Product product);

    Task<bool> UpdateProductAsync(Product product);

    Task<bool> DeleteProductAsync(string id);
    #endregion
}
