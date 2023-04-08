using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using Trinity.Application.Contracts;
using Trinity.Application.DTOs.Products;
using Trinity.Application.Exceptions.Products;
using Trinity.Domain.Entities.Products;
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

            await this.productsBasePersistence.AddAsync(product);
            return productOutput;
        }

        public async Task<ProductsOutput> UpdateAsync(ProductsUpdateInput productInput, string id)
        {
            Products? product = await this.productStaticPersistence.GetByIdAsync(id) ?? throw new ProductsException("Product not found.");
            product.Name = productInput.Name;
            product.Description = productInput.Description;
            product.ImageUrl = productInput.ImageUrl;
            product.Quantity = productInput.Quantity;
            product.Price = productInput.Price;
            product.Discount = productInput.Discount;

            ProductsOutput productOutput = this.mapper.Map<ProductsOutput>(product);

            await this.productsBasePersistence.UpdateAsync(product);
            return productOutput;
        }

        public async Task<ProductsOutput> DeleteAsync(string id)
        {
            Products? product = await this.productStaticPersistence.GetByIdAsync(id) ?? throw new ProductsException("Product not found.");
            ProductsOutput productOutput = this.mapper.Map<ProductsOutput>(product);

            await this.productsBasePersistence.DeleteAsync(id);
            return productOutput;
        }
    }
}
