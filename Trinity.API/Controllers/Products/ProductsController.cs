using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Trinity.API.Extensions;
using Trinity.API.ViewModels;
using Trinity.Application.Contracts;
using Trinity.Application.DTOs.Products;

namespace Trinity.API.Controllers.Product
{
    [ApiController]
    [Route("v1/[Controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsService productService;

        public ProductsController(IProductsService productService)
        {
            this.productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            try
            {
                IEnumerable<ProductsOutput> products = await this.productService.GetAsync();
                return StatusCode((int)HttpStatusCode.OK, new ResultViewModel<IEnumerable<ProductsOutput>>(products));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ResultViewModel<IEnumerable<ProductsOutput>>(ex.Message));
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] ProductsAddInput product)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ResultViewModel<ProductsOutput>(ModelState.GetErrors()));
                }

                ProductsOutput productAdded = await this.productService.AddAsync(product);
                return StatusCode((int)HttpStatusCode.Created, new ResultViewModel<ProductsOutput>(productAdded));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ResultViewModel<ProductsOutput>(ex.Message));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync([FromBody] ProductsUpdateInput productInput, [FromRoute] string id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ResultViewModel<ProductsOutput>(ModelState.GetErrors()));
                }

                ProductsOutput? productUpdate = await this.productService.UpdateAsync(productInput, id);
                return StatusCode((int)HttpStatusCode.OK, new ResultViewModel<ProductsOutput?>(productUpdate));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ResultViewModel<ProductsOutput>(ex.Message));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] string id)
        {
            try
            {
                ProductsOutput? productDeleted = await this.productService.DeleteAsync(id);
                return StatusCode((int)HttpStatusCode.OK, new ResultViewModel<ProductsOutput?>(productDeleted));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ResultViewModel<ProductsOutput>(ex.Message));
            }
        }
    }
}
