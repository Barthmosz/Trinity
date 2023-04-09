using System.Collections.Generic;
using Trinity.Application.DTOs.Account;
using Trinity.Domain.Entities;

namespace Trinity.Test.Factories
{
    public static class AccountFactory
    {
        public static AccountSignUpInput MakeAccountSignUpInput()
        {
            return new AccountSignUpInput()
            {
                Name = "any_name",
                Email = "any_email@mail.com",
                Password = "any_password"
            };
        }

        public static AccountSignInInput MakeAccountSignInInput()
        {
            return new AccountSignInInput()
            {
                Email = "any_email@mail.com",
                Password = "any_password"
            };
        }

        public static Account MakeAccount()
        {
            return new Account()
            {
                Name = "any_name",
                Email = "any_email@mail.com",
                PasswordHash = "any_password_hash",
                Roles = new List<string>()
                {
                    "user"
                }
            };

        }
    }
}
