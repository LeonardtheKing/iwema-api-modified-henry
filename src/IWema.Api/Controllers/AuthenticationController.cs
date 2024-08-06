using IWema.Application.AdminLogin.Command;
using IWema.Application.Login.Command;
using IWema.Application.Logout;
using IWema.Infrastructure.Adapters.ActiveDirectory;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace IWema.Api.Controllers;


[Route("api/[controller]")]
[ApiController]
public class AuthenticationController(IMediator mediator,IOptions<ActiveDirectoryConfigOptions> options) : BaseController
{
    private readonly ActiveDirectoryConfigOptions _options=options.Value;

    
    [HttpPost]
    [Route("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginInputModel request)
    {

        if (!request.Email.Contains(_options.Domain))
        {
            request.Email = request.Email + _options.Domain;
        }
        var command = new LoginCommand(request.Email, request.Password);
        var response = await mediator.Send(command);

        return ServiceResponse(response);
    }

    [HttpPost]
    [Route("login/admin")]
    [AllowAnonymous]
    public async Task<IActionResult> LoginAdmin([FromBody] AdminLoginInput request)
    {

        if (!request.Email.Contains(_options.Domain))
        {
            request.Email = request.Email + _options.Domain;
        }
        var command = new AdminLoginCommand(request.Email, request.Password);
        var response = await mediator.Send(command);

        return ServiceResponse(response);
    }

    [HttpDelete]
    [Route("refresh-token")]
    [AllowAnonymous]
    public async Task<IActionResult> LogoutUser(LoginInputModel request)
    {
        var command = new LoginCommand(request.Email, request.Password);
        var response = await mediator.Send(command);

        return ServiceResponse(response);
    }

    [AllowAnonymous]
    [HttpDelete]
    [Route("logout")]
    public async Task<IActionResult> LogoutUser([FromBody] string token)
    {
        var command = new LogoutCommand(token);
        var response = await mediator.Send(command);

        return ServiceResponse(response);
    }

}
