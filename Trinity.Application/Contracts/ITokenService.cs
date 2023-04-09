using Trinity.Application.DTOs.Account;
using Trinity.Domain.Entities;

namespace Trinity.Application.Contracts
{
    public interface ITokenService
    {
        TokenOutput GenerateToken(Account account);
    }
}
