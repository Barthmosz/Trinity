using System;

namespace Trinity.Application.Exceptions
{
    public class AccountException : Exception
    {
        public AccountException(string errorMessage) : base(errorMessage) { }
    }
}
