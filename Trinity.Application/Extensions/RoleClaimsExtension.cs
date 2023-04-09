using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Claims;
using Trinity.Domain.Entities;

namespace Trinity.Application.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class RoleClaimsExtension
    {
        [Authorize]
        public static IEnumerable<Claim> GetClaims(this Account account)
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
