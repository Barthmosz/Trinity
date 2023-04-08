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
        private const string productId = "any_id";

        private readonly Mock<IStaticPersistence<Products>> productsStaticPersistence = new();
        private readonly Mock<IBasePersistence<Products>> productsBasePersistence = new();
        private readonly Mock<ITokenService> tokenService = new();
        private readonly Mock<IMapper> mapper = new();

        private IProductsService productsService;

        private ProductsAddInput productAddInput;
        private ProductsUpdateInput productUpdateInput;
        private ProductsOutput productOutput;
        private IEnumerable<ProductsOutput> productsOutput;

        private Products? product;
        private IEnumerable<Products> products;

        [SetUp]
        public void SetUp()
        {
            this.productAddInput = new()
            {
                Name = "any_name",
                Description = "any_description",
                Quantity = 1,
                ImageUrl = "any_image_url",
                Price = 10
            };
            this.productUpdateInput = new()
            {
                Name = "any_name",
                Description = "any_description",
                Quantity = 1,
                Discount = 1,
                ImageUrl = "any_image_url",
                Price = 10
            };
            this.product = new()
            {
                Name = "any_name",
                Description = "any_description",
                Quantity = 1,
                Discount = 1,
                ImageUrl = "any_image_url",
                Price = 10
            };
            this.products = new List<Products>() { this.product };
            this.productOutput = new()
            {
                Id = "any_id",
                Name = "any_name",
                Description = "any_description",
                Quantity = 1,
                Price = 10,
                ImageUrl = "any_image_url",
                Discount = 1
            };
            this.productsOutput = new List<ProductsOutput>() { this.productOutput };

            this.mapper.Setup(m => m.Map<IEnumerable<ProductsOutput>>(this.products)).Returns(this.productsOutput);
            this.mapper.Setup(m => m.Map<Products>(this.productAddInput)).Returns(this.product);
            this.mapper.Setup(m => m.Map<ProductsOutput>(this.product)).Returns(this.productOutput);

            this.productsStaticPersistence.Setup(p => p.GetAllAsync()).Returns(Task.FromResult(this.products));
            this.productsStaticPersistence.Setup(p => p.GetByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(this.product)!);

            this.productsService = new ProductsService(this.productsStaticPersistence.Object, this.productsBasePersistence.Object, this.mapper.Object);
        }

        #region GetAsync
        [Test]
        public async Task GetAsync_Returns_All_Products()
        {
            IEnumerable<ProductsOutput> result = await this.productsService.GetAsync();
            Assert.That(result, Is.EqualTo(this.productsOutput));
        }
        #endregion

        #region AddAsync
        [Test]
        public async Task AddAsync_Returns_Created_Product()
        {
            ProductsOutput result = await this.productsService.AddAsync(this.productAddInput);
            Assert.That(result, Is.EqualTo(this.productOutput));
        }
        #endregion

        #region UpdateAsync
        [Test]
        public void UpdateAsync_Throws_If_Product_Does_Not_Exists()
        {
            this.product = null;
            this.productsStaticPersistence.Setup(p => p.GetByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(this.product));

            Assert.ThrowsAsync<ProductsException>(async () => await this.productsService.UpdateAsync(this.productUpdateInput, productId));
        }

        [Test]
        public async Task UpdateAsync_Returns_Updated_Product()
        {
            ProductsOutput? result = await this.productsService.UpdateAsync(this.productUpdateInput, productId);
            Assert.That(result, Is.EqualTo(this.productOutput));
        }
        #endregion
    }
}
