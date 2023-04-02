using System.Collections.Generic;
using System.Threading.Tasks;
using Trinity.Application.DTOs.Products;

namespace Trinity.Application.Contracts
{
    public interface IProductsService
    {
        Task<IEnumerable<ProductsOutput>> GetProductsAsync();
        Task<ProductsOutput> AddProductAsync(ProductsInput product);
        Task<ProductsOutput?> UpdateProductAsync(ProductsInput product, string id);
        Task<ProductsOutput?> DeleteProductAsync(string id);
    }
}
