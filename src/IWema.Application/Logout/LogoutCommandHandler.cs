using IWema.Application.Common.DTO;
using IWema.Application.Contract;
using IWema.Domain.Entity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace IWema.Application.Logout;

public record LogoutCommand(string token) : IRequest<ServiceResponse>;

public class LogoutCommandHandler : IRequestHandler<LogoutCommand, ServiceResponse>
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IJwtTokenManager _jwtTokenManager;
    private readonly IUserSessionRepository _userSessionRepository;

    public LogoutCommandHandler(
        SignInManager<ApplicationUser> signInManager,
        IJwtTokenManager jwtTokenManager,
        IUserSessionRepository userSessionRepository
    )
    {
        _signInManager = signInManager;
        _jwtTokenManager = jwtTokenManager;
        _userSessionRepository = userSessionRepository;
    }

    public async Task<ServiceResponse> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        // Revoke the JWT
        _jwtTokenManager.RevokeToken(request.token);

        // Get the user id from the token
        var principal = _jwtTokenManager.GetPrincipalFromExpiredToken(request.token);
        var userIdClaim = principal?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        if (userIdClaim != null)
        {
            var userLoginSession = await _userSessionRepository.GetLatestSessionByUserIdAsync(userIdClaim);

            if (userLoginSession != null)
            {
                // Set LogoutTime to the current UTC time
                var logoutTime = DateTime.UtcNow;
                userLoginSession.UpdateLogoutTime(logoutTime);

                // Calculate the duration in seconds
                if (userLoginSession.LogoutTime.HasValue)
                {
                    var durationInSeconds = (logoutTime - userLoginSession.LoginTime).TotalSeconds;
                    var duration = TimeSpan.FromSeconds(durationInSeconds);
                    userLoginSession.UpdateDurationInSeconds(duration);
                }

                userLoginSession.UpdateIsActive(false);

                // Update the existing session in the database
                await _userSessionRepository.UpdateSessionAsync(userLoginSession);
            }
        }

        await _signInManager.SignOutAsync();
        return new ServiceResponse("Successfully logged out", true);
    }
}
