using IWema.Application.ManagementTeam.Command.Add;
using IWema.Application.ManagementTeam.Command.Delete;
using IWema.Application.ManagementTeam.Command.Update;
using IWema.Application.ManagementTeam.Query.GetAll;
using IWema.Application.ManagementTeam.Query.GetById;
using IWema.Domain.Entity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IWema.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ManagementTeamController(IMediator mediator) : BaseController
{
    [Authorize(Roles = Role.ADMIN)]
    [HttpPost]
    public async Task<IActionResult> Add([FromForm] AddManagementTeamCommandInputModel request)
    {
        var command = new AddManagementTeamCommand(request.File, request.nameOfExecutive, request.position, request.quote, request.profileLink);
        var response = await mediator.Send(command);
        return ServiceResponse(response);
    }

    [Authorize(Roles = Role.ADMIN)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var response = await mediator.Send(new DeleteManagementTeamCommand(id));
        return ServiceResponse(response);
    }

    [Authorize(Roles = Role.ADMIN)]
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var response = await mediator.Send(new GetManagementTeamByIdQuery(id));
        return ServiceResponse(response);
    }

    [Authorize(Roles = Role.ADMIN)]
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var response = await mediator.Send(new GetAllManageentTeamQuery());
        return ServiceResponse(response);
    }

    [Authorize(Roles = Role.ADMIN)]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromForm] UpdateManagementTeamInputModel request)
    {
        UpdateManagementTeamCommand command = new(id, request.Id, request.File, request.nameOfExecutive, request.position, request.quote, request.profileLink);
        var response = await mediator.Send(command);

        return ServiceResponse(response);
    }

}








