using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Trinity.API.Extensions;
using Trinity.API.ViewModels;
using Trinity.Application.Contracts;
using Trinity.Application.DTOs.Product;
using Trinity.Application.Exceptions;

namespace Trinity.API.Controllers
{
    [ApiController]
    [Route("v1/[Controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService ProductService;

        public ProductController(IProductService productService)
        {
            ProductService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            try
            {
                IEnumerable<ProductOutput> products = await ProductService.GetAsync();
                return StatusCode((int)HttpStatusCode.OK, new ResultViewModel<IEnumerable<ProductOutput>>(products));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ResultViewModel<string>(ex.Message));
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] ProductAddInput productAddInput)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ResultViewModel<ProductOutput>(ModelState.GetErrors()));
                }

                ProductOutput productAdded = await ProductService.AddAsync(productAddInput);
                return StatusCode((int)HttpStatusCode.Created, new ResultViewModel<ProductOutput>(productAdded));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ResultViewModel<string>(ex.Message));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync([FromBody] ProductUpdateInput productUpdateInput, [FromRoute] string id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ResultViewModel<ProductOutput>(ModelState.GetErrors()));
                }

                ProductOutput? productUpdate = await ProductService.UpdateAsync(productUpdateInput, id);
                return StatusCode((int)HttpStatusCode.OK, new ResultViewModel<ProductOutput?>(productUpdate));
            }
            catch (ProductException ex)
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
                ProductOutput? productDeleted = await ProductService.DeleteAsync(id);
                return StatusCode((int)HttpStatusCode.OK, new ResultViewModel<ProductOutput?>(productDeleted));
            }
            catch (ProductException ex)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, new ResultViewModel<ProductOutput>(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ResultViewModel<string>(ex.Message));
            }
        }
    }
}
