using System.Threading.Tasks;
using Trinity.Application.DTOs.Accounts;
using Trinity.Application.DTOs.Users;

namespace Trinity.Application.Contracts
{
    public interface IAccountsService
    {
        Task<AccountsOutput> SignUpAsync(AccountsInput user);
        Task<TokenOutput> SignInAsync(AccountsInput user);
    }
}
