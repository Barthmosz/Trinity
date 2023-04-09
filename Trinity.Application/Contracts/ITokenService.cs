using Trinity.Application.DTOs.Accounts;
using Trinity.Domain.Entities.Accounts;

namespace Trinity.Application.Contracts
{
    public interface ITokenService
    {
        TokenOutput GenerateToken(Accounts account);
    }
}
