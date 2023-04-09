using System.Collections.Generic;
using System.Threading.Tasks;
using Trinity.Application.DTOs.Product;

namespace Trinity.Application.Contracts
{
    public interface IProductService
    {
        Task<IEnumerable<ProductOutput>> GetAsync();
        Task<ProductOutput> AddAsync(ProductAddInput productInput);
        Task<ProductOutput> UpdateAsync(ProductUpdateInput productInput, string id);
        Task<ProductOutput> DeleteAsync(string id);
    }
}
