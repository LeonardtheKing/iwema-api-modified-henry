//using IWema.Application.Common.DTO;
//using IWema.Application.Contract;
//using IWema.Domain.Entity;
//using MediatR;
//using Microsoft.AspNetCore.Identity;

//namespace IWema.Application.Logout;

//public record LogoutCommand(string token) : IRequest<ServiceResponse>;

//public class LogoutCommandHandler(SignInManager<ApplicationUser> signInManager, IJwtTokenManager jwtTokenManager) : IRequestHandler<LogoutCommand, ServiceResponse>
//{
//    public async Task<ServiceResponse> Handle(LogoutCommand request, CancellationToken cancellationToken)
//    {
//        // Revoke the JWT
//        jwtTokenManager.RevokeToken(request.token);
//        await signInManager.SignOutAsync();
//        return new("Successfully logged out", true);
//    }
//}


using IWema.Application.Common.DTO;
using IWema.Application.Contract;
using IWema.Domain.Entity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace IWema.Application.Logout;

public record LogoutCommand(string token) : IRequest<ServiceResponse>;

public class LogoutCommandHandler(
    SignInManager<ApplicationUser> signInManager,
    IJwtTokenManager jwtTokenManager,
    IUserSessionRepository userSessionRepository
) : IRequestHandler<LogoutCommand, ServiceResponse>
{
    public async Task<ServiceResponse> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        // Revoke the JWT
        jwtTokenManager.RevokeToken(request.token);

        // Get the user id from the token
        var principal = jwtTokenManager.GetPrincipalFromExpiredToken(request.token);
        var userIdClaim = principal?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        if (userIdClaim != null)
        {
            var userLoginSession = await userSessionRepository.GetLatestSessionByUserIdAsync(userIdClaim);

            if (userLoginSession != null)
            {
                userLoginSession.LogoutTime = DateTime.UtcNow;
                userLoginSession.DurationInSeconds = (userLoginSession.LogoutTime.Value - userLoginSession.LoginTime).TotalSeconds;
                await userSessionRepository.UpdateSessionAsync(userLoginSession);
            }
        }

        await signInManager.SignOutAsync();
        return new("Successfully logged out", true);
    }
}

