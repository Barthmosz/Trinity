using System;

namespace Trinity.Application.Exceptions.Products
{
    public class ProductsException : Exception
    {
        public ProductsException(string errorMessage) : base(errorMessage) { }
    }
}
