using AutoMapper;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using Trinity.Application.Contracts;
using Trinity.Application.DTOs.Products;
using Trinity.Application.Exceptions.Products;
using Trinity.Application.Services;
using Trinity.Domain.Entities.Products;
using Trinity.Persistence.Contracts;

namespace Trinity.Test.Application.Services.Product
{
    public class ProductsServiceTest
    {
        private const string ProductId = "any_id";

        private readonly Mock<IStaticPersistence<Products>> ProductsStaticPersistence = new();
        private readonly Mock<IDynamicPersistence<Products>> ProductsBasePersistence = new();
        private readonly Mock<IMapper> Mapper = new();

        private IProductsService ProductsService;

        private ProductsAddInput ProductAddInput;
        private ProductsUpdateInput ProductUpdateInput;
        private ProductsOutput ProductOutput;
        private IEnumerable<ProductsOutput> ProductsOutput;

        private Products? Product;
        private IEnumerable<Products> Products;

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
            Products = new List<Products>() { Product };
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
            ProductsOutput = new List<ProductsOutput>() { ProductOutput };

            Mapper.Setup(m => m.Map<IEnumerable<ProductsOutput>>(Products)).Returns(ProductsOutput);
            Mapper.Setup(m => m.Map<Products>(ProductAddInput)).Returns(Product);
            Mapper.Setup(m => m.Map<ProductsOutput>(Product)).Returns(ProductOutput);

            ProductsStaticPersistence.Setup(p => p.GetAllAsync()).Returns(Task.FromResult(Products));
            ProductsStaticPersistence.Setup(p => p.GetByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(Product)!);

            ProductsService = new ProductsService(ProductsStaticPersistence.Object, ProductsBasePersistence.Object, Mapper.Object);
        }

        #region GetAsync
        [Test]
        public async Task GetAsync_Returns_All_Products()
        {
            IEnumerable<ProductsOutput> result = await ProductsService.GetAsync();
            Assert.That(result, Is.EqualTo(ProductsOutput));
        }
        #endregion

        #region AddAsync
        [Test]
        public async Task AddAsync_Returns_Created_Product()
        {
            ProductsOutput result = await ProductsService.AddAsync(ProductAddInput);
            Assert.That(result, Is.EqualTo(ProductOutput));
        }
        #endregion

        #region UpdateAsync
        [Test]
        public void UpdateAsync_Throws_If_Product_Does_Not_Exists()
        {
            Product = null;
            ProductsStaticPersistence.Setup(p => p.GetByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(Product));

            Assert.ThrowsAsync<ProductsException>(async () => await ProductsService.UpdateAsync(ProductUpdateInput, ProductId));
        }

        [Test]
        public async Task UpdateAsync_Returns_Updated_Product()
        {
            ProductsOutput? result = await ProductsService.UpdateAsync(ProductUpdateInput, ProductId);
            Assert.That(result, Is.EqualTo(ProductOutput));
        }
        #endregion

        #region DeleteAsync
        [Test]
        public void DeleteAsync_Throws_If_Product_Does_Not_Exists()
        {
            Product = null;
            ProductsStaticPersistence.Setup(p => p.GetByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(Product));

            Assert.ThrowsAsync<ProductsException>(async () => await ProductsService.DeleteAsync(ProductId));
        }

        [Test]
        public async Task DeleteAsync_Returns_Deleted_Product()
        {
            ProductsOutput? result = await ProductsService.DeleteAsync(ProductId);
            Assert.That(result, Is.EqualTo(ProductOutput));
        }
        #endregion
    }
}
