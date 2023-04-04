using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using Trinity.Application.Contracts;
using Trinity.Application.DTOs.Products;
using Trinity.Domain.Products;
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

        public async Task<IEnumerable<ProductsOutput>> GetAsync()
        {
            IEnumerable<Products> products = await this.productStaticPersistence.GetAllAsync();
            IEnumerable<ProductsOutput> productsOutputs = this.mapper.Map<IEnumerable<ProductsOutput>>(products);
            return productsOutputs;
        }

        public async Task<ProductsOutput> AddAsync(ProductsAddInput productInput)
        {
            Products product = this.mapper.Map<Products>(productInput);
            ProductsOutput productOutput = this.mapper.Map<ProductsOutput>(product);

            await this.productsBasePersistence.Add(product);
            return productOutput;
        }

        public async Task<ProductsOutput?> UpdateAsync(ProductsUpdateInput productInput, string id)
        {
            Products? product = await this.productStaticPersistence.GetByIdAsync(id);

            if (product != null)
            {
                product.Name = productInput.Name;
                product.Description = productInput.Description;
                product.Image = productInput.Image;
                product.Quantity = productInput.Quantity;
                product.Price = productInput.Price;
                product.Discount = productInput.Discount;

                ProductsOutput productOutput = this.mapper.Map<ProductsOutput>(product);

                await this.productsBasePersistence.Update(product);
                return productOutput;
            }

            return null;
        }

        public async Task<ProductsOutput?> DeleteAsync(string id)
        {
            Products? product = await this.productStaticPersistence.GetByIdAsync(id);

            if (product != null)
            {
                ProductsOutput productOutput = this.mapper.Map<ProductsOutput>(product);

                await this.productsBasePersistence.Delete(id);
                return productOutput;
            }

            return null;
        }
    }
}
