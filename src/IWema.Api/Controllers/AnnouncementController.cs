using IWema.Application.Announcements.Command.Add;
using IWema.Application.Announcements.Command.Delete;
using IWema.Application.Announcements.Command.PartialUpdate;
using IWema.Application.Announcements.Query.GetAnnouncementById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IWema.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AnnouncementController(IMediator mediator) : BaseController
{
    [HttpPost]
    public async Task<IActionResult> Add([FromForm] AddAnnouncementInputModel request)
    {
        var command = new AddAnnouncementCommand(request.File, request.Title, request.Date, request.Content, request.Link);
        var response = await mediator.Send(command);
        return ServiceResponse(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var response = await mediator.Send(new DeleteAnnouncementCommand(id));
        return ServiceResponse(response);
    }

    [HttpGet]
    [AllowAnonymous]

    public async Task<IActionResult> Get()
    {
        var response = await mediator.Send(new GetAllAnnouncementQuery());
        return ServiceResponse(response);
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var response = await mediator.Send(new GetAnnouncementByIdQuery(id));
        return ServiceResponse(response);
    }

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
