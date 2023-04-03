using System.Threading.Tasks;
using Trinity.Application.DTOs.Users;

namespace Trinity.Application.Contracts
{
    public interface IAccountsService
    {
        Task<AccountsOutput> SignUpAsync(AccountsInput user);
        Task<string> SignInAsync(AccountsInput user);
    }
}
