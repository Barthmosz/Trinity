using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;
using Trinity.Application.Contracts;

namespace Trinity.API.Controllers.Product
{
    [ApiController]
    [Route("api")]
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
                var products = await this.productService.GetProductsAsync();
                return StatusCode((int)HttpStatusCode.OK, new { data = products });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { error = ex.Message });
            }
        }
    }
}
