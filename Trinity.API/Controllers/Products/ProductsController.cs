﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Trinity.Application.Contracts;
using Trinity.Application.DTOs;
using Trinity.Domain;

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
                IEnumerable<Products> products = await this.productService.GetProductsAsync();
                return StatusCode((int)HttpStatusCode.OK, new { data = products });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { error = ex.Message });
            }
        }

        [HttpPost("/v1/products")]
        public async Task<IActionResult> AddProductAsync([FromBody] Products product)
        {
            try
            {
                Products productAdded = await this.productService.AddProductAsync(product);
                return StatusCode((int)HttpStatusCode.Created, new { data = productAdded });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { error = ex.Message });
            }
        }

        [HttpPut("/v1/products")]
        public async Task<IActionResult> UpdateProductAsync([FromBody] ProductsDTO product)
        {
            try
            {
                Products? productUpdate = await this.productService.UpdateProductAsync(product);
                return StatusCode((int)HttpStatusCode.OK, new { data = productUpdate });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { error = ex.Message });
            }
        }

        [HttpDelete("/v1/products/{id}")]
        public async Task<IActionResult> DeleteProductAsync(string id)
        {
            try
            {
                Products? productDeleted = await this.productService.DeleteProductAsync(id);
                return StatusCode((int)HttpStatusCode.OK, new { data = productDeleted });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { error = ex.Message });
            }
        }
    }
}
