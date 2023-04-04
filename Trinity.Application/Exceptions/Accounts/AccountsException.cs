using System;

namespace Trinity.Application.Exceptions.Accounts
{
    public class AccountsException : Exception
    {
        public AccountsException(string errorMessage) : base(errorMessage) { }
    }
}
