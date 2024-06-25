﻿using IWema.Application.Login.AdminLogin.Command;
using IWema.Application.Login.Command;
using IWema.Application.Logout;
using IWema.Infrastructure.Adapters.ActiveDirectory;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;

namespace IWema.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController(IMediator mediator,IOptions<ActiveDirectoryConfigOptions> options) : BaseController
    {
        private readonly ActiveDirectoryConfigOptions _options=options.Value;

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
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

        [AllowAnonymous]
        [HttpPost]
        [Route("login/admin")]
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
        public async Task<IActionResult> LogoutUser(LoginInputModel request)
        {
            var command = new LoginCommand(request.Email, request.Password);
            var response = await mediator.Send(command);

            return ServiceResponse(response);
        }

        [HttpDelete]
        [Route("logout")]
        public async Task<IActionResult> LogoutUser([FromBody] string token)
        {
            var command = new LogoutCommand(token);
            var response = await mediator.Send(command);

            return ServiceResponse(response);
        }

    }
}