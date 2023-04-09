using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Trinity.API.Controllers.Product;
using Trinity.Application.Contracts;
using Trinity.Application.DTOs.Products;
using Trinity.Application.Services;
using Trinity.Domain.Entities.Products;
using Trinity.Persistence.Contracts;

namespace Trinity.Test.API.Controllers.Product
{
    public class ProductsControllerTest
    {
        private const string productId = "any_id";

        private readonly Mock<IStaticPersistence<Products>> productsStaticPersistence = new();
        private readonly Mock<IBasePersistence<Products>> productsBasePersistence = new();
        private readonly Mock<IMapper> mapper = new();

        private ProductsController productsController;
        private IProductsService productsService;

        private Products? product;
        private IEnumerable<Products> products;

        private ProductsAddInput productsAddInput;
        private ProductsUpdateInput productsUpdateInput;

        [SetUp]
        public void SetUp()
        {
            this.productsAddInput = new()
            {
                Name = "any_name",
                Description = "any_description",
                ImageUrl = "any_image_url",
                Price = 1,
                Quantity = 1
            };
            this.productsUpdateInput = new()
            {
                Name = "any_name",
                Description = "any_description",
                ImageUrl = "any_image_url",
                Price = 1,
                Quantity = 1,
                Discount = 0.5m
            };
            this.product = new()
            {
                Name = "any_name",
                Description = "any_description",
                ImageUrl = "any_image_url",
                Price = 1,
                Quantity = 1,
                Discount = 1
            };
            this.products = new List<Products>()
            {
                product
            };

            this.productsStaticPersistence.Setup(p => p.GetAllAsync()).Returns(Task.FromResult(this.products));
            this.productsStaticPersistence.Setup(p => p.GetByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(this.product)!);
            this.productsBasePersistence.Setup(p => p.AddAsync(It.IsAny<Products>())).Returns(Task.FromResult(true));
            this.productsBasePersistence.Setup(p => p.UpdateAsync(It.IsAny<Products>())).Returns(Task.FromResult(true));
            this.productsBasePersistence.Setup(p => p.DeleteAsync(It.IsAny<string>())).Returns(Task.FromResult(true));

            this.productsService = new ProductsService(this.productsStaticPersistence.Object, this.productsBasePersistence.Object, this.mapper.Object);
            this.productsController = new(this.productsService);
        }

        #region GetAsync
        [Test]
        public async Task GetAsync_Should_Return_Ok_If_Persistence_Returns_Products()
        {
            ObjectResult? result = await this.productsController.GetAsync() as ObjectResult;
            Assert.That(result!.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));
        }

        [Test]
        public async Task GetAsync_Should_Return_InternalServerError_If_Persistence_Throws()
        {
            this.productsStaticPersistence.Setup(p => p.GetAllAsync()).Throws(new Exception());

            ObjectResult? result = await this.productsController.GetAsync() as ObjectResult;
            Assert.That(result!.StatusCode, Is.EqualTo((int)HttpStatusCode.InternalServerError));
        }
        #endregion

        #region AddAsync
        [Test]
        public async Task AddAsync_Should_Return_BadRequest_If_Input_Is_Invalid()
        {
            this.productsController.ModelState.AddModelError("name", "Name is required.");
            ObjectResult? result = await this.productsController.AddAsync(this.productsAddInput) as ObjectResult;
            Assert.That(result!.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task AddAsync_Should_Return_Created_If_Persistence_Returns_True()
        {
            ObjectResult? result = await this.productsController.AddAsync(this.productsAddInput) as ObjectResult;
            Assert.That(result!.StatusCode, Is.EqualTo((int)HttpStatusCode.Created));
        }

        [Test]
        public async Task AddAsync_Should_Return_InternalServerError_If_Persistence_Throws()
        {
            this.productsBasePersistence.Setup(p => p.AddAsync(It.IsAny<Products>())).Throws(new Exception());

            ObjectResult? result = await this.productsController.AddAsync(this.productsAddInput) as ObjectResult;
            Assert.That(result!.StatusCode, Is.EqualTo((int)HttpStatusCode.InternalServerError));
        }
        #endregion

        #region UpdateAsync
        [Test]
        public async Task UpdateAsync_Should_Return_BadRequest_If_Input_Is_Invalid()
        {
            this.productsController.ModelState.AddModelError("name", "Name is required.");
            ObjectResult? result = await this.productsController.UpdateAsync(this.productsUpdateInput, productId) as ObjectResult;
            Assert.That(result!.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task UpdateAsync_Should_Return_Ok_If_Persistence_Returns_True()
        {
            ObjectResult? result = await this.productsController.UpdateAsync(this.productsUpdateInput, productId) as ObjectResult;
            Assert.That(result!.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));
        }

        [Test]
        public async Task UpdateAsync_Should_Return_BadRequest_If_Product_Does_Not_Exists()
        {
            this.product = null;
            this.productsStaticPersistence.Setup(p => p.GetByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(this.product));

            ObjectResult? result = await this.productsController.UpdateAsync(this.productsUpdateInput, productId) as ObjectResult;
            Assert.That(result!.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task UpdateAsync_Should_Return_InternalServerError_If_Persistence_Throws()
        {
            this.productsBasePersistence.Setup(p => p.UpdateAsync(It.IsAny<Products>())).Throws(new Exception());

            ObjectResult? result = await this.productsController.UpdateAsync(this.productsUpdateInput, productId) as ObjectResult;
            Assert.That(result!.StatusCode, Is.EqualTo((int)HttpStatusCode.InternalServerError));
        }
        #endregion

        #region DeleteAsync
        [Test]
        public async Task DeleteAsync_Should_Return_BadRequest_If_Product_Does_Not_Exists()
        {
            this.product = null;
            this.productsStaticPersistence.Setup(p => p.GetByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(this.product));

            ObjectResult? result = await this.productsController.DeleteAsync(productId) as ObjectResult;
            Assert.That(result!.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task DeleteAsync_Should_Return_Ok_If_Persistence_Returns_True()
        {
            ObjectResult? result = await this.productsController.DeleteAsync(productId) as ObjectResult;
            Assert.That(result!.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));
        }

        [Test]
        public async Task DeleteAsync_Should_Return_InternalServerError_If_Persistence_Throws()
        {
            this.productsBasePersistence.Setup(p => p.DeleteAsync(It.IsAny<string>())).Throws(new Exception());

            ObjectResult? result = await this.productsController.DeleteAsync(productId) as ObjectResult;
            Assert.That(result!.StatusCode, Is.EqualTo((int)HttpStatusCode.InternalServerError));
        }
        #endregion
    }
}
