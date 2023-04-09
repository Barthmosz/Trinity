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
        private const string ProductId = "any_id";

        private readonly Mock<IStaticPersistence<Products>> ProductsStaticPersistence = new();
        private readonly Mock<IDynamicPersistence<Products>> ProductsBasePersistence = new();
        private readonly Mock<IMapper> Mapper = new();

        private ProductsController ProductsController;
        private IProductsService ProductsService;

        private Products? Product;
        private IEnumerable<Products> Products;

        private ProductsAddInput ProductsAddInput;
        private ProductsUpdateInput ProductsUpdateInput;

        [SetUp]
        public void SetUp()
        {
            ProductsAddInput = new()
            {
                Name = "any_name",
                Description = "any_description",
                ImageUrl = "any_image_url",
                Price = 1,
                Quantity = 1
            };
            ProductsUpdateInput = new()
            {
                Name = "any_name",
                Description = "any_description",
                ImageUrl = "any_image_url",
                Price = 1,
                Quantity = 1,
                Discount = 0.5m
            };
            Product = new()
            {
                Name = "any_name",
                Description = "any_description",
                ImageUrl = "any_image_url",
                Price = 1,
                Quantity = 1,
                Discount = 1
            };
            Products = new List<Products>()
            {
                Product
            };

            ProductsStaticPersistence.Setup(p => p.GetAllAsync()).Returns(Task.FromResult(Products));
            ProductsStaticPersistence.Setup(p => p.GetByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(Product)!);
            ProductsBasePersistence.Setup(p => p.AddAsync(It.IsAny<Products>())).Returns(Task.FromResult(true));
            ProductsBasePersistence.Setup(p => p.UpdateAsync(It.IsAny<Products>())).Returns(Task.FromResult(true));
            ProductsBasePersistence.Setup(p => p.DeleteAsync(It.IsAny<string>())).Returns(Task.FromResult(true));

            ProductsService = new ProductsService(ProductsStaticPersistence.Object, ProductsBasePersistence.Object, Mapper.Object);
            ProductsController = new(ProductsService);
        }

        #region GetAsync
        [Test]
        public async Task GetAsync_Should_Return_Ok_If_Persistence_Returns_Products()
        {
            ObjectResult? result = await ProductsController.GetAsync() as ObjectResult;
            Assert.That(result!.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));
        }

        [Test]
        public async Task GetAsync_Should_Return_InternalServerError_If_Persistence_Throws()
        {
            ProductsStaticPersistence.Setup(p => p.GetAllAsync()).Throws(new Exception());

            ObjectResult? result = await ProductsController.GetAsync() as ObjectResult;
            Assert.That(result!.StatusCode, Is.EqualTo((int)HttpStatusCode.InternalServerError));
        }
        #endregion

        #region AddAsync
        [Test]
        public async Task AddAsync_Should_Return_BadRequest_If_Input_Is_Invalid()
        {
            ProductsController.ModelState.AddModelError("name", "Name is required.");
            ObjectResult? result = await ProductsController.AddAsync(ProductsAddInput) as ObjectResult;
            Assert.That(result!.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task AddAsync_Should_Return_Created_If_Persistence_Returns_True()
        {
            ObjectResult? result = await ProductsController.AddAsync(ProductsAddInput) as ObjectResult;
            Assert.That(result!.StatusCode, Is.EqualTo((int)HttpStatusCode.Created));
        }

        [Test]
        public async Task AddAsync_Should_Return_InternalServerError_If_Persistence_Throws()
        {
            ProductsBasePersistence.Setup(p => p.AddAsync(It.IsAny<Products>())).Throws(new Exception());

            ObjectResult? result = await ProductsController.AddAsync(ProductsAddInput) as ObjectResult;
            Assert.That(result!.StatusCode, Is.EqualTo((int)HttpStatusCode.InternalServerError));
        }
        #endregion

        #region UpdateAsync
        [Test]
        public async Task UpdateAsync_Should_Return_BadRequest_If_Input_Is_Invalid()
        {
            ProductsController.ModelState.AddModelError("name", "Name is required.");
            ObjectResult? result = await ProductsController.UpdateAsync(ProductsUpdateInput, ProductId) as ObjectResult;
            Assert.That(result!.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task UpdateAsync_Should_Return_Ok_If_Persistence_Returns_True()
        {
            ObjectResult? result = await ProductsController.UpdateAsync(ProductsUpdateInput, ProductId) as ObjectResult;
            Assert.That(result!.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));
        }

        [Test]
        public async Task UpdateAsync_Should_Return_BadRequest_If_Product_Does_Not_Exists()
        {
            Product = null;
            ProductsStaticPersistence.Setup(p => p.GetByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(Product));

            ObjectResult? result = await ProductsController.UpdateAsync(ProductsUpdateInput, ProductId) as ObjectResult;
            Assert.That(result!.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task UpdateAsync_Should_Return_InternalServerError_If_Persistence_Throws()
        {
            ProductsBasePersistence.Setup(p => p.UpdateAsync(It.IsAny<Products>())).Throws(new Exception());

            ObjectResult? result = await ProductsController.UpdateAsync(ProductsUpdateInput, ProductId) as ObjectResult;
            Assert.That(result!.StatusCode, Is.EqualTo((int)HttpStatusCode.InternalServerError));
        }
        #endregion

        #region DeleteAsync
        [Test]
        public async Task DeleteAsync_Should_Return_BadRequest_If_Product_Does_Not_Exists()
        {
            Product = null;
            ProductsStaticPersistence.Setup(p => p.GetByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(Product));

            ObjectResult? result = await ProductsController.DeleteAsync(ProductId) as ObjectResult;
            Assert.That(result!.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task DeleteAsync_Should_Return_Ok_If_Persistence_Returns_True()
        {
            ObjectResult? result = await ProductsController.DeleteAsync(ProductId) as ObjectResult;
            Assert.That(result!.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));
        }

        [Test]
        public async Task DeleteAsync_Should_Return_InternalServerError_If_Persistence_Throws()
        {
            ProductsBasePersistence.Setup(p => p.DeleteAsync(It.IsAny<string>())).Throws(new Exception());

            ObjectResult? result = await ProductsController.DeleteAsync(ProductId) as ObjectResult;
            Assert.That(result!.StatusCode, Is.EqualTo((int)HttpStatusCode.InternalServerError));
        }
        #endregion
    }
}
