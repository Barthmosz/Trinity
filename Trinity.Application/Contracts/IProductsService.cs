﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Trinity.Application.DTOs;
using Trinity.Domain;

namespace Trinity.Application.Contracts
{
    public interface IProductsService
    {
        Task<IEnumerable<Products>> GetProductsAsync();
        Task<Products> AddProductAsync(Products product);
        Task<Products?> UpdateProductAsync(ProductsDTO product);
    }
}
