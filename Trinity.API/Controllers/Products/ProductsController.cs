using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Trinity.API.ViewModels;
using Trinity.Application.Contracts;
using Trinity.Application.DTOs.Products;

namespace Trinity.API.Controllers.Product
{
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsService productService;

        public ProductsController(IProductsService productService)
        {
            this.productService = productService;
        }

        [HttpGet("/v1/products")]
        public async Task<IActionResult> GetProductsAsync()
        {
            try
            {
                IEnumerable<ProductsOutput> products = await this.productService.GetProductsAsync();
                return StatusCode((int)HttpStatusCode.OK, new ResultViewModel<IEnumerable<ProductsOutput>>(products));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ResultViewModel<IEnumerable<ProductsOutput>>(ex.Message));
            }
        }

        [HttpPost("/v1/products")]
        public async Task<IActionResult> AddProductAsync([FromBody] ProductsInput product)
        {
            try
            {
                ProductsOutput productAdded = await this.productService.AddProductAsync(product);
                return StatusCode((int)HttpStatusCode.Created, new ResultViewModel<ProductsOutput>(productAdded));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ResultViewModel<ProductsOutput>(ex.Message));
            }
        }

        [HttpPut("/v1/products/{id}")]
        public async Task<IActionResult> UpdateProductAsync([FromBody] ProductsInput product, [FromRoute] string id)
        {
            try
            {
                ProductsOutput? productUpdate = await this.productService.UpdateProductAsync(product, id);
                return StatusCode((int)HttpStatusCode.OK, new ResultViewModel<ProductsOutput?>(productUpdate));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ResultViewModel<ProductsOutput>(ex.Message));
            }
        }

        [HttpDelete("/v1/products/{id}")]
        public async Task<IActionResult> DeleteProductAsync([FromRoute] string id)
        {
            try
            {
                ProductsOutput? productDeleted = await this.productService.DeleteProductAsync(id);
                return StatusCode((int)HttpStatusCode.OK, new ResultViewModel<ProductsOutput?>(productDeleted));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ResultViewModel<ProductsOutput>(ex.Message));
            }
        }
    }
}
