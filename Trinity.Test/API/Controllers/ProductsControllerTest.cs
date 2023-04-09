using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Trinity.API.Controllers;
using Trinity.Application.Contracts;
using Trinity.Application.DTOs.Product;
using Trinity.Application.Services;
using Trinity.Domain.Entities;
using Trinity.Persistence.Contracts;
using Trinity.Test._Factories;

namespace Trinity.Test.API.Controllers
{
    [TestFixture]
    public class ProductControllerTest
    {
        private const string ProductId = "any_id";

        private readonly Mock<IStaticPersistence<Product>> ProductStaticPersistence = new();
        private readonly Mock<IDynamicPersistence<Product>> ProductBasePersistence = new();
        private readonly Mock<IMapper> Mapper = new();

        private ProductController ProductController;
        private IProductService ProductService;

        private ProductAddInput ProductAddInput;
        private ProductUpdateInput ProductUpdateInput;

        private Product? Product;
        private IEnumerable<Product> Products;        

        [SetUp]
        public void SetUp()
        {
            ProductAddInput = ProductFactory.MakeProductAddInput();
            ProductUpdateInput = ProductFactory.MakeProductUpdateInput();
            Product = ProductFactory.MakeProduct();
            Products = new List<Product>()
            {
                Product
            };

            ProductStaticPersistence.Setup(p => p.GetAllAsync()).Returns(Task.FromResult(Products));
            ProductStaticPersistence.Setup(p => p.GetByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(Product)!);
            ProductBasePersistence.Setup(p => p.AddAsync(It.IsAny<Product>())).Returns(Task.FromResult(true));
            ProductBasePersistence.Setup(p => p.UpdateAsync(It.IsAny<Product>())).Returns(Task.FromResult(true));
            ProductBasePersistence.Setup(p => p.DeleteAsync(It.IsAny<string>())).Returns(Task.FromResult(true));

            ProductService = new ProductService(ProductStaticPersistence.Object, ProductBasePersistence.Object, Mapper.Object);
            ProductController = new(ProductService);
        }

        #region GetAsync
        [Test]
        public async Task GetAsync_Should_Return_Ok_If_Persistence_Returns_Products()
        {
            ObjectResult? result = await ProductController.GetAsync() as ObjectResult;
            Assert.That(result!.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));
        }

        [Test]
        public async Task GetAsync_Should_Return_InternalServerError_If_Persistence_Throws()
        {
            ProductStaticPersistence.Setup(p => p.GetAllAsync()).Throws(new Exception());

            ObjectResult? result = await ProductController.GetAsync() as ObjectResult;
            Assert.That(result!.StatusCode, Is.EqualTo((int)HttpStatusCode.InternalServerError));
        }
        #endregion

        #region AddAsync
        [Test]
        public async Task AddAsync_Should_Return_BadRequest_If_Input_Is_Invalid()
        {
            ProductController.ModelState.AddModelError("name", "Name is required.");
            ObjectResult? result = await ProductController.AddAsync(ProductAddInput) as ObjectResult;
            Assert.That(result!.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task AddAsync_Should_Return_Created_If_Persistence_Returns_True()
        {
            ObjectResult? result = await ProductController.AddAsync(ProductAddInput) as ObjectResult;
            Assert.That(result!.StatusCode, Is.EqualTo((int)HttpStatusCode.Created));
        }

        [Test]
        public async Task AddAsync_Should_Return_InternalServerError_If_Persistence_Throws()
        {
            ProductBasePersistence.Setup(p => p.AddAsync(It.IsAny<Product>())).Throws(new Exception());

            ObjectResult? result = await ProductController.AddAsync(ProductAddInput) as ObjectResult;
            Assert.That(result!.StatusCode, Is.EqualTo((int)HttpStatusCode.InternalServerError));
        }
        #endregion

        #region UpdateAsync
        [Test]
        public async Task UpdateAsync_Should_Return_BadRequest_If_Input_Is_Invalid()
        {
            ProductController.ModelState.AddModelError("name", "Name is required.");
            ObjectResult? result = await ProductController.UpdateAsync(ProductUpdateInput, ProductId) as ObjectResult;
            Assert.That(result!.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task UpdateAsync_Should_Return_Ok_If_Persistence_Returns_True()
        {
            ObjectResult? result = await ProductController.UpdateAsync(ProductUpdateInput, ProductId) as ObjectResult;
            Assert.That(result!.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));
        }

        [Test]
        public async Task UpdateAsync_Should_Return_BadRequest_If_Product_Does_Not_Exists()
        {
            Product = null;
            ProductStaticPersistence.Setup(p => p.GetByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(Product));

            ObjectResult? result = await ProductController.UpdateAsync(ProductUpdateInput, ProductId) as ObjectResult;
            Assert.That(result!.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task UpdateAsync_Should_Return_InternalServerError_If_Persistence_Throws()
        {
            ProductBasePersistence.Setup(p => p.UpdateAsync(It.IsAny<Product>())).Throws(new Exception());

            ObjectResult? result = await ProductController.UpdateAsync(ProductUpdateInput, ProductId) as ObjectResult;
            Assert.That(result!.StatusCode, Is.EqualTo((int)HttpStatusCode.InternalServerError));
        }
        #endregion

        #region DeleteAsync
        [Test]
        public async Task DeleteAsync_Should_Return_BadRequest_If_Product_Does_Not_Exists()
        {
            Product = null;
            ProductStaticPersistence.Setup(p => p.GetByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(Product));

            ObjectResult? result = await ProductController.DeleteAsync(ProductId) as ObjectResult;
            Assert.That(result!.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task DeleteAsync_Should_Return_Ok_If_Persistence_Returns_True()
        {
            ObjectResult? result = await ProductController.DeleteAsync(ProductId) as ObjectResult;
            Assert.That(result!.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));
        }

        [Test]
        public async Task DeleteAsync_Should_Return_InternalServerError_If_Persistence_Throws()
        {
            ProductBasePersistence.Setup(p => p.DeleteAsync(It.IsAny<string>())).Throws(new Exception());

            ObjectResult? result = await ProductController.DeleteAsync(ProductId) as ObjectResult;
            Assert.That(result!.StatusCode, Is.EqualTo((int)HttpStatusCode.InternalServerError));
        }
        #endregion
    }
}
