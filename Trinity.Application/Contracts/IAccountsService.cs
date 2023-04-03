using System.Threading.Tasks;
using Trinity.Application.DTOs.Users;

namespace Trinity.Application.Contracts
{
    public interface IAccountsService
    {
        Task<UsersOutput> SignUpAccountAsync(UsersInput user);
        Task<string> SignInAccountAsync(UsersInput user);
    }
}
