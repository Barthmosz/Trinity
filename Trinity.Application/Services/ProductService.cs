using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using Trinity.Application.Contracts;
using Trinity.Application.DTOs.Product;
using Trinity.Application.Exceptions;
using Trinity.Domain.Entities;
using Trinity.Persistence.Contracts;

namespace Trinity.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IStaticPersistence<Product> ProductStaticPersistence;
        private readonly IDynamicPersistence<Product> ProductsBasePersistence;
        private readonly IMapper Mapper;

        public ProductService(
            IStaticPersistence<Product> productStaticPersistence,
            IDynamicPersistence<Product> productsBasePersistence,
            IMapper mapper)
        {
            ProductStaticPersistence = productStaticPersistence;
            ProductsBasePersistence = productsBasePersistence;
            Mapper = mapper;
        }

        public async Task<IEnumerable<ProductOutput>> GetAsync()
        {
            IEnumerable<Product> products = await ProductStaticPersistence.GetAllAsync();
            IEnumerable<ProductOutput> productsOutputs = Mapper.Map<IEnumerable<ProductOutput>>(products);
            return productsOutputs;
        }

        public async Task<ProductOutput> AddAsync(ProductAddInput productAddInput)
        {
            Product product = Mapper.Map<Product>(productAddInput);
            ProductOutput productOutput = Mapper.Map<ProductOutput>(product);

            await ProductsBasePersistence.AddAsync(product);
            return productOutput;
        }

        public async Task<ProductOutput> UpdateAsync(ProductUpdateInput productUpdateInput, string id)
        {
            Product? product = await ProductStaticPersistence.GetByIdAsync(id) ?? throw new ProductException("Product not found.");
            product.Name = productUpdateInput.Name;
            product.Description = productUpdateInput.Description;
            product.ImageUrl = productUpdateInput.ImageUrl;
            product.Quantity = productUpdateInput.Quantity;
            product.Price = productUpdateInput.Price;
            product.Discount = productUpdateInput.Discount;

            ProductOutput productOutput = Mapper.Map<ProductOutput>(product);

            await ProductsBasePersistence.UpdateAsync(product);
            return productOutput;
        }

        public async Task<ProductOutput> DeleteAsync(string id)
        {
            Product? product = await ProductStaticPersistence.GetByIdAsync(id) ?? throw new ProductException("Product not found.");
            ProductOutput productOutput = Mapper.Map<ProductOutput>(product);

            await ProductsBasePersistence.DeleteAsync(id);
            return productOutput;
        }
    }
}
