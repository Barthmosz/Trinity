using AutoMapper;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using Trinity.Application.Contracts;
using Trinity.Application.DTOs.Product;
using Trinity.Application.Exceptions;
using Trinity.Application.Services;
using Trinity.Domain.Entities;
using Trinity.Persistence.Contracts;

namespace Trinity.Test.Application.Services
{
    public class ProductServiceTest
    {
        private const string ProductId = "any_id";

        private readonly Mock<IStaticPersistence<Product>> ProductStaticPersistence = new();
        private readonly Mock<IDynamicPersistence<Product>> ProductBasePersistence = new();
        private readonly Mock<IMapper> Mapper = new();

        private IProductService ProductService;

        private ProductAddInput ProductAddInput;
        private ProductUpdateInput ProductUpdateInput;
        private ProductOutput ProductOutput;
        private IEnumerable<ProductOutput> ProductsOutput;

        private Product? Product;
        private IEnumerable<Product> Products;

        [SetUp]
        public void SetUp()
        {
            ProductAddInput = new()
            {
                Name = "any_name",
                Description = "any_description",
                Quantity = 1,
                ImageUrl = "any_image_url",
                Price = 10
            };
            ProductUpdateInput = new()
            {
                Name = "any_name",
                Description = "any_description",
                Quantity = 1,
                Discount = 1,
                ImageUrl = "any_image_url",
                Price = 10
            };
            Product = new()
            {
                Name = "any_name",
                Description = "any_description",
                Quantity = 1,
                Discount = 1,
                ImageUrl = "any_image_url",
                Price = 10
            };
            Products = new List<Product>() { Product };
            ProductOutput = new()
            {
                Id = "any_id",
                Name = "any_name",
                Description = "any_description",
                Quantity = 1,
                Price = 10,
                ImageUrl = "any_image_url",
                Discount = 1
            };
            ProductsOutput = new List<ProductOutput>() { ProductOutput };

            Mapper.Setup(m => m.Map<IEnumerable<ProductOutput>>(Products)).Returns(ProductsOutput);
            Mapper.Setup(m => m.Map<Product>(ProductAddInput)).Returns(Product);
            Mapper.Setup(m => m.Map<ProductOutput>(Product)).Returns(ProductOutput);

            ProductStaticPersistence.Setup(p => p.GetAllAsync()).Returns(Task.FromResult(Products));
            ProductStaticPersistence.Setup(p => p.GetByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(Product)!);

            ProductService = new ProductService(ProductStaticPersistence.Object, ProductBasePersistence.Object, Mapper.Object);
        }

        #region GetAsync
        [Test]
        public async Task GetAsync_Returns_All_Products()
        {
            IEnumerable<ProductOutput> result = await ProductService.GetAsync();
            Assert.That(result, Is.EqualTo(ProductsOutput));
        }
        #endregion

        #region AddAsync
        [Test]
        public async Task AddAsync_Returns_Created_Product()
        {
            ProductOutput result = await ProductService.AddAsync(ProductAddInput);
            Assert.That(result, Is.EqualTo(ProductOutput));
        }
        #endregion

        #region UpdateAsync
        [Test]
        public void UpdateAsync_Throws_If_Product_Does_Not_Exists()
        {
            Product = null;
            ProductStaticPersistence.Setup(p => p.GetByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(Product));

            Assert.ThrowsAsync<ProductException>(async () => await ProductService.UpdateAsync(ProductUpdateInput, ProductId));
        }

        [Test]
        public async Task UpdateAsync_Returns_Updated_Product()
        {
            ProductOutput? result = await ProductService.UpdateAsync(ProductUpdateInput, ProductId);
            Assert.That(result, Is.EqualTo(ProductOutput));
        }
        #endregion

        #region DeleteAsync
        [Test]
        public void DeleteAsync_Throws_If_Product_Does_Not_Exists()
        {
            Product = null;
            ProductStaticPersistence.Setup(p => p.GetByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(Product));

            Assert.ThrowsAsync<ProductException>(async () => await ProductService.DeleteAsync(ProductId));
        }

        [Test]
        public async Task DeleteAsync_Returns_Deleted_Product()
        {
            ProductOutput? result = await ProductService.DeleteAsync(ProductId);
            Assert.That(result, Is.EqualTo(ProductOutput));
        }
        #endregion
    }
}
