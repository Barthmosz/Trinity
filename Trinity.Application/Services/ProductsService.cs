using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using Trinity.Application.Contracts;
using Trinity.Application.DTOs.Products;
using Trinity.Domain;
using Trinity.Persistence.Contracts;

namespace Trinity.Application.Services
{
    public class ProductsService : IProductsService
    {
        private readonly IStaticPersistence<Products> productStaticPersistence;
        private readonly IBasePersistence<Products> productsBasePersistence;
        private readonly IMapper mapper;

        public ProductsService(
            IStaticPersistence<Products> productStaticPersistence,
            IBasePersistence<Products> productsBasePersistence,
            IMapper mapper)
        {
            this.productStaticPersistence = productStaticPersistence;
            this.productsBasePersistence = productsBasePersistence;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<Products>> GetProductsAsync()
        {
            return await this.productStaticPersistence.GetAllAsync();
        }

        public async Task<ProductsOutput> AddProductAsync(ProductsInput product)
        {
            Products productToBeAdded = this.mapper.Map<Products>(product);
            ProductsOutput productOutput = this.mapper.Map<ProductsOutput>(productToBeAdded);

            await this.productsBasePersistence.Add(productToBeAdded);
            return productOutput;
        }

        public async Task<ProductsOutput?> UpdateProductAsync(ProductsInput productInput, string id)
        {
            Products? productToUpdate = await this.productStaticPersistence.GetByIdAsync(id);

            if (productToUpdate != null)
            {
                Products product = this.mapper.Map<Products>(productInput);
                ProductsOutput productOutput = this.mapper.Map<ProductsOutput>(product);

                await this.productsBasePersistence.Update(product);
                return productOutput;
            }

            return null;
        }

        public async Task<ProductsOutput?> DeleteProductAsync(string id)
        {
            Products? productToDelete = await this.productStaticPersistence.GetByIdAsync(id);

            if (productToDelete != null)
            {
                ProductsOutput productOutput = this.mapper.Map<ProductsOutput>(productToDelete);

                await this.productsBasePersistence.Delete(id);
                return productOutput;
            }

            return null;
        }
    }
}
