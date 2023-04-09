using System.Threading.Tasks;
using Trinity.Application.DTOs.Account;

namespace Trinity.Application.Contracts
{
    public interface IAccountService
    {
        Task<AccountOutput> SignUpAsync(AccountSignUpInput accountSignUpInput);
        Task<TokenOutput> SignInAsync(AccountSignInInput accountSignInInput);
    }
}
