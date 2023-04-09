using System.Threading.Tasks;
using Trinity.Application.DTOs.Accounts;

namespace Trinity.Application.Contracts
{
    public interface IAccountsService
    {
        Task<AccountsOutput> SignUpAsync(AccountsSignUpInput accountSignUpInput);
        Task<TokenOutput> SignInAsync(AccountsSignInInput accountSignInInput);
    }
}
