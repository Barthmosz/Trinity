using Trinity.Application.DTOs.Accounts;
using Trinity.Domain.Accounts;

namespace Trinity.Application.Contracts
{
    public interface ITokenService
    {
        TokenOutput GenerateToken(Accounts user);
    }
}
