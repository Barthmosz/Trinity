using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Trinity.API.Extensions;
using Trinity.API.ViewModels;
using Trinity.Application.Contracts;
using Trinity.Application.DTOs.Products;
using Trinity.Application.Exceptions.Products;

namespace Trinity.API.Controllers.Product
{
    [ApiController]
    [Route("v1/[Controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsService ProductService;

        public ProductsController(IProductsService productService)
        {
            ProductService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            try
            {
                IEnumerable<ProductsOutput> products = await ProductService.GetAsync();
                return StatusCode((int)HttpStatusCode.OK, new ResultViewModel<IEnumerable<ProductsOutput>>(products));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ResultViewModel<string>(ex.Message));
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

                ProductsOutput productAdded = await ProductService.AddAsync(product);
                return StatusCode((int)HttpStatusCode.Created, new ResultViewModel<ProductsOutput>(productAdded));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ResultViewModel<string>(ex.Message));
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

                ProductsOutput? productUpdate = await ProductService.UpdateAsync(productInput, id);
                return StatusCode((int)HttpStatusCode.OK, new ResultViewModel<ProductsOutput?>(productUpdate));
            }
            catch (ProductsException ex)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, new ResultViewModel<string>(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ResultViewModel<string>(ex.Message));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] string id)
        {
            try
            {
                ProductsOutput? productDeleted = await ProductService.DeleteAsync(id);
                return StatusCode((int)HttpStatusCode.OK, new ResultViewModel<ProductsOutput?>(productDeleted));
            }
            catch (ProductsException ex)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, new ResultViewModel<ProductsOutput>(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ResultViewModel<string>(ex.Message));
            }
        }
    }
}
