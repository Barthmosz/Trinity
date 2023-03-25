using System.Collections.Generic;
using System.Threading.Tasks;
using Trinity.Application.Contracts;
using Trinity.Domain;
using Trinity.Persistence.Contracts;

namespace Trinity.Application.Services
{
    public class ProductsService : IProductsService
    {
        private readonly IStaticPersistence<Products> productStaticPersistence;

        public ProductsService(IStaticPersistence<Products> productStaticPersistence)
        {
            this.productStaticPersistence = productStaticPersistence;
        }

        public async Task<IEnumerable<Products>> GetProductsAsync()
        {
            return await this.productStaticPersistence.GetAllAsync();
        }
    }
}
