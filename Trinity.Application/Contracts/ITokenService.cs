using Trinity.Domain.Accounts;

namespace Trinity.Application.Contracts
{
    public interface ITokenService
    {
        string GenerateToken(Accounts user);
    }
}
