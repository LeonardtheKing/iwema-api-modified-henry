//using IWema.Application.Common.Configuration;
//using IWema.Application.Contract;
//using IWema.Domain.Entity;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.Extensions.Options;
//using Microsoft.IdentityModel.Tokens;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Security.Cryptography;
//using System.Text;

//public class JwtTokenManager(IOptions<JwtConfigOptions> options, UserManager<ApplicationUser> userManager) : IJwtTokenManager
//{
//    private static readonly Dictionary<string, DateTime> RevokedTokens = new Dictionary<string, DateTime>();
//    private readonly JwtConfigOptions _options = options.Value;
//    public string GenerateJWT(List<Claim> claims)
//    {
//        var authSecret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Secret));

//        // Add jti claim to uniquely identify the token
//        var jti = Guid.NewGuid().ToString();
//        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, jti));

//        var tokenObject = new JwtSecurityToken(
//            issuer: _options.ValidIssuer,
//            audience: _options.ValidAudience,
//            expires: DateTime.Now.AddMinutes(30),
//            claims: claims,
//            signingCredentials: new SigningCredentials(authSecret, SecurityAlgorithms.HmacSha256)
//        );

//        string token = new JwtSecurityTokenHandler().WriteToken(tokenObject);

//        return token;
//    }

//    public async Task<string> GenerateRefreshToken()
//    {
//        await Task.Delay(0);
//        var randomNumber = new byte[32];
//        using var rng = RandomNumberGenerator.Create();
//        rng.GetBytes(randomNumber);
//        return Convert.ToBase64String(randomNumber);
//    }

//    public void RevokeToken(string token)
//    {
//        var tokenHandler = new JwtSecurityTokenHandler();
//        JwtSecurityToken jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

//        if (jwtToken != null)
//        {
//            string jti = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
//            if (jti != null)
//            {
//                RevokedTokens[jti] = jwtToken.ValidTo;
//            }
//        }
//    }

//    public bool IsTokenRevoked(string token)
//    {
//        var tokenHandler = new JwtSecurityTokenHandler();
//        JwtSecurityToken jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

//        if (jwtToken != null)
//        {
//            string jti = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
//            if (jti != null)
//            {
//                return RevokedTokens.ContainsKey(jti);
//            }
//        }
//        return false;
//    }
//}

using IWema.Application.Common.Configuration;
using IWema.Application.Contract;
using IWema.Domain.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

public class JwtTokenManager(IOptions<JwtConfigOptions> options, UserManager<ApplicationUser> userManager) : IJwtTokenManager
{
    private static readonly Dictionary<string, DateTime> RevokedTokens = new Dictionary<string, DateTime>();
    private readonly JwtConfigOptions _options = options.Value;

    public string GenerateJWT(List<Claim> claims)
    {
        var authSecret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Secret));

        // Add jti claim to uniquely identify the token
        var jti = Guid.NewGuid().ToString();
        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, jti));

        var tokenObject = new JwtSecurityToken(
            issuer: _options.ValidIssuer,
            audience: _options.ValidAudience,
            expires: DateTime.Now.AddMinutes(30),
            claims: claims,
            signingCredentials: new SigningCredentials(authSecret, SecurityAlgorithms.HmacSha256)
        );

        string token = new JwtSecurityTokenHandler().WriteToken(tokenObject);

        return token;
    }

    public async Task<string> GenerateRefreshToken()
    {
        await Task.Delay(0);
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public void RevokeToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        JwtSecurityToken jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

        if (jwtToken != null)
        {
            string jti = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
            if (jti != null)
            {
                RevokedTokens[jti] = jwtToken.ValidTo;
            }
        }
    }

    public bool IsTokenRevoked(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        JwtSecurityToken jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

        if (jwtToken != null)
        {
            string jti = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
            if (jti != null)
            {
                return RevokedTokens.ContainsKey(jti);
            }
        }
        return false;
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Secret)),
            ClockSkew = TimeSpan.Zero
        }, out SecurityToken securityToken);

        var jwtToken = securityToken as JwtSecurityToken;
        if (jwtToken == null || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }

        return principal;
    }
}



