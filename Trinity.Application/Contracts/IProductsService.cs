using System.Collections.Generic;
using System.Threading.Tasks;
using Trinity.Domain;

namespace Trinity.Application.Contracts
{
    public interface IProductsService
    {
        Task<IEnumerable<Products>> GetProductsAsync();
    }
}
