using System.Threading.Tasks;
using Trinity.Application.DTOs.Users;

namespace Trinity.Application.Contracts
{
    public interface IUsersService
    {
        Task<UsersOutput> AddUserAsync(UsersInput user);
    }
}
