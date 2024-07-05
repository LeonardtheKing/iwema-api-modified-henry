using IWema.Application.Birthday.Query.GetBirthdays;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IWema.Api.Controllers;

public class BirthdayController(IMediator mediator) : BaseController
{
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var serviceResponse = await mediator.Send(new GetBirthdaysQuery());
        return ServiceResponse(serviceResponse);
    }
}