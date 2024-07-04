using IWema.Application.Announcements.Command.Add;
using IWema.Application.Announcements.Command.Delete;
using IWema.Application.Announcements.Command.PartialUpdate;
using IWema.Application.Announcements.Query.GetAnnouncementById;
using IWema.Domain.Entity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IWema.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AnnouncementController(IMediator mediator) : BaseController
{

    [Authorize(Roles = Role.ADMIN)]
    [HttpPost]
    public async Task<IActionResult> Add([FromForm] AddAnnouncementInputModel request)
    {
        var command = new AddAnnouncementCommand(request.File, request.Title, request.Date, request.Content, request.Link);
        var response = await mediator.Send(command);
        return ServiceResponse(response);
    }

    [Authorize(Roles = Role.ADMIN)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var response = await mediator.Send(new DeleteAnnouncementCommand(id));
        return ServiceResponse(response);
    }

    [Authorize(Roles = Role.ADMIN)]
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var response = await mediator.Send(new GetAllAnnouncementQuery());
        return ServiceResponse(response);
    }

    [Authorize(Roles = Role.ADMIN)]
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var response = await mediator.Send(new GetAnnouncementByIdQuery(id));
        return ServiceResponse(response);
    }

    [Authorize(Roles = Role.ADMIN)]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAnnouncement(Guid id,
        [FromForm] PartiallyUpdateAnnouncementInputModel request)
    {
        PartiallyUpdateAnnouncementCommand command = new(request.Id, request.Title, request.Date, request.File,
            request.Content, request.Link);
        var result = await mediator.Send(command);
        return ServiceResponse(result);
    }
}
