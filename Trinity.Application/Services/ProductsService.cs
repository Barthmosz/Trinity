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
        private readonly IBasePersistence<Products> productsBasePersistence;

        public ProductsService(IStaticPersistence<Products> productStaticPersistence, IBasePersistence<Products> productsBasePersistence)
        {
            this.productStaticPersistence = productStaticPersistence;
            this.productsBasePersistence = productsBasePersistence;
        }

        public async Task<IEnumerable<Products>> GetProductsAsync()
        {
            return await this.productStaticPersistence.GetAllAsync();
        }

        public async Task<bool> AddProductAsync(Products product)
        {
            return await this.productsBasePersistence.Add(product);
        }
    }
}
