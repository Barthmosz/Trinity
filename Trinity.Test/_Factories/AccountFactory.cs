using Trinity.Application.DTOs.Account;

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
    }
}
