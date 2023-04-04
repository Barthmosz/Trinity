using System.Collections.Generic;
using System.Threading.Tasks;
using Trinity.Application.DTOs.Products;

namespace Trinity.Application.Contracts
{
    public interface IProductsService
    {
        Task<IEnumerable<ProductsOutput>> GetAsync();
        Task<ProductsOutput> AddAsync(ProductsAddInput productInput);
        Task<ProductsOutput?> UpdateAsync(ProductsUpdateInput productInput, string id);
        Task<ProductsOutput?> DeleteAsync(string id);
    }
}
