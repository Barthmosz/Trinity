using System.Collections.Generic;
using System.Threading.Tasks;
using Trinity.Application.DTOs.Products;
using Trinity.Domain;

namespace Trinity.Application.Contracts
{
    public interface IProductsService
    {
        Task<IEnumerable<Products>> GetProductsAsync();
        Task<ProductsOutput> AddProductAsync(ProductsInput product);
        Task<ProductsOutput?> UpdateProductAsync(ProductsInput product, string id);
        Task<ProductsOutput?> DeleteProductAsync(string id);
    }
}
