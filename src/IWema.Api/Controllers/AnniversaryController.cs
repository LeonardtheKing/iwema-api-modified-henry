using IWema.Application.Anniversary.Query.GetAnniversaries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IWema.Api.Controllers;

public class AnniversaryController(IMediator mediator) : BaseController
{
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var serviceResponse = await mediator.Send(new GetAnniversariesQuery());
        return ServiceResponse(serviceResponse);
    }
}
