using System.Security.Claims;

namespace IWema.Application.Contract;

public interface IJwtTokenManager
{
    string GenerateJWT(List<Claim> claims);
    Task<string> GenerateRefreshToken();
    void RevokeToken(string token);
    bool IsTokenRevoked(string token);
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);

}
