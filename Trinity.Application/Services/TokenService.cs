using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Trinity.Application.Contracts;
using Trinity.Application.DTOs.Accounts;
using Trinity.Application.Extensions;
using Trinity.Domain.Entities.Accounts;

namespace Trinity.Application.Services
{
    [ExcludeFromCodeCoverage]
    public class TokenService : ITokenService
    {
        private readonly IConfiguration Configuration;

        public TokenService(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public TokenOutput GenerateToken(Accounts account)
        {
            JwtSecurityTokenHandler tokenHandler = new();
            byte[] key = Encoding.ASCII.GetBytes(Configuration.GetSection("JwtKey").Value!);
            IEnumerable<Claim> claims = account.GetClaims();
            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            TokenOutput tokenOutput = new()
            {
                Token = tokenHandler.WriteToken(token)
            };
            return tokenOutput;
        }
    }
}
