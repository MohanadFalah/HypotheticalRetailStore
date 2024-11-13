using System;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Net;
namespace HypotheticalRetailStore.Helper;

public static class TokenGenerator
{
    public static string GenerateToken(DateTime Expiration, Guid userid, string role)
    {
        Claim roleClaim = new Claim(ClaimTypes.Role, role);
        long iat = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, JWTCredentials.xSubject),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, iat.ToString()),
            new Claim("Id", userid.ToString()),
            roleClaim
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTCredentials.xKey));

        var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        //
        var token = new JwtSecurityToken(
            JWTCredentials.xIssuer,
            JWTCredentials.xAudidance,
            claims,
            expires: Expiration,
            signingCredentials: signIn);
        return (new JwtSecurityTokenHandler().WriteToken(token));
    }
}