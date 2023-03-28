using System.Collections.Generic;
using System.Threading.Tasks;
using Trinity.Application.DTOs;
using Trinity.Domain;

namespace Trinity.Application.Contracts
{
    public interface IProductsService
    {
        Task<IEnumerable<Products>> GetProductsAsync();
        Task<ProductsDTO> AddProductAsync(ProductsDTO product);
        Task<Products?> UpdateProductAsync(ProductsDTO product);
        Task<Products?> DeleteProductAsync(string id);
    }
}
