using System.Collections.Generic;
using System.Threading.Tasks;
using Trinity.Application.DTOs.Products;

namespace Trinity.Application.Contracts
{
    public interface IProductsService
    {
        Task<IEnumerable<ProductsOutput>> GetAsync();
        Task<ProductsOutput> AddAsync(ProductsInput product);
        Task<ProductsOutput?> UpdateAsync(ProductsInput product, string id);
        Task<ProductsOutput?> DeleteAsync(string id);
    }
}
