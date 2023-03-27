using System.Collections.Generic;
using System.Threading.Tasks;
using Trinity.Application.Contracts;
using Trinity.Application.DTOs;
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

        public async Task<Products> AddProductAsync(Products product)
        {
            await this.productsBasePersistence.Add(product);
            return product;
        }

        public async Task<Products?> UpdateProductAsync(ProductsDTO product)
        {
            Products? productToUpdate = await this.productStaticPersistence.GetByIdAsync(product.Id);

            if (productToUpdate != null)
            {
                productToUpdate.Image = product.Image;
                productToUpdate.Name = product.Name;
                productToUpdate.Description = product.Description;
                productToUpdate.Discount = product.Discount;
                productToUpdate.Quantity = product.Quantity;
                productToUpdate.Price = product.Price;

                await this.productsBasePersistence.Update(productToUpdate);
                return productToUpdate;
            }

            return productToUpdate;
        }

        public async Task<Products?> DeleteProductAsync(string id)
        {
            Products? productToDelete = await this.productStaticPersistence.GetByIdAsync(id);

            if (productToDelete != null)
            {
                await this.productsBasePersistence.Delete(id);
                return productToDelete;
            }

            return productToDelete;
        }
    }
}
