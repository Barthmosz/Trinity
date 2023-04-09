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
        private readonly IStaticPersistence<Products> ProductStaticPersistence;
        private readonly IDynamicPersistence<Products> ProductsBasePersistence;
        private readonly IMapper Mapper;

        public ProductsService(
            IStaticPersistence<Products> productStaticPersistence,
            IDynamicPersistence<Products> productsBasePersistence,
            IMapper mapper)
        {
            ProductStaticPersistence = productStaticPersistence;
            ProductsBasePersistence = productsBasePersistence;
            Mapper = mapper;
        }

        public async Task<IEnumerable<ProductsOutput>> GetAsync()
        {
            IEnumerable<Products> products = await ProductStaticPersistence.GetAllAsync();
            IEnumerable<ProductsOutput> productsOutputs = Mapper.Map<IEnumerable<ProductsOutput>>(products);
            return productsOutputs;
        }

        public async Task<ProductsOutput> AddAsync(ProductsAddInput productInput)
        {
            Products product = Mapper.Map<Products>(productInput);
            ProductsOutput productOutput = Mapper.Map<ProductsOutput>(product);

            await ProductsBasePersistence.AddAsync(product);
            return productOutput;
        }

        public async Task<ProductsOutput> UpdateAsync(ProductsUpdateInput productInput, string id)
        {
            Products? product = await ProductStaticPersistence.GetByIdAsync(id) ?? throw new ProductsException("Product not found.");
            product.Name = productInput.Name;
            product.Description = productInput.Description;
            product.ImageUrl = productInput.ImageUrl;
            product.Quantity = productInput.Quantity;
            product.Price = productInput.Price;
            product.Discount = productInput.Discount;

            ProductsOutput productOutput = Mapper.Map<ProductsOutput>(product);

            await ProductsBasePersistence.UpdateAsync(product);
            return productOutput;
        }

        public async Task<ProductsOutput> DeleteAsync(string id)
        {
            Products? product = await ProductStaticPersistence.GetByIdAsync(id) ?? throw new ProductsException("Product not found.");
            ProductsOutput productOutput = Mapper.Map<ProductsOutput>(product);

            await ProductsBasePersistence.DeleteAsync(id);
            return productOutput;
        }
    }
}
