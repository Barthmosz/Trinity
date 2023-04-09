using System;

namespace Trinity.Application.Exceptions
{
    public class ProductException : Exception
    {
        public ProductException(string errorMessage) : base(errorMessage) { }
    }
}
