using IWema.Application.Common.DTO;
using IWema.Application.Contract;
using IWema.Domain.Entity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace IWema.Application.Logout;

public record LogoutCommand(string token) : IRequest<ServiceResponse>;

public class LogoutCommandHandler(SignInManager<ApplicationUser> signInManager, IJwtTokenManager jwtTokenManager) : IRequestHandler<LogoutCommand, ServiceResponse>
{
    public async Task<ServiceResponse> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        // Revoke the JWT
        jwtTokenManager.RevokeToken(request.token);
        await signInManager.SignOutAsync();
        return new("Successfully logged out", true);
    }


}