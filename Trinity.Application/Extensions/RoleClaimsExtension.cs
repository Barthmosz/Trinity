using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Trinity.Domain.Entities.Accounts;

namespace Trinity.Application.Extensions
{
    public static class RoleClaimsExtension
    {
        [Authorize]
        public static IEnumerable<Claim> GetClaims(this Accounts account)
        {
            List<Claim> result = new()
            {
                new(ClaimTypes.Name, account.Email)
            };

            result.AddRange(account.Roles.Select(role => new Claim(ClaimTypes.Role, role)));
            return result;
        }
    }
}
