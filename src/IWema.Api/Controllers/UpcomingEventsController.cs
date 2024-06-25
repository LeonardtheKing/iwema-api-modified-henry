using IWema.Application.UpcomingEvents.Command.Add;
using IWema.Application.UpcomingEvents.Command.Delete;
using IWema.Application.UpcomingEvents.Command.PartialUpdate;
using IWema.Application.UpcomingEvents.Command.Update;
using IWema.Application.UpcomingEvents.Query.GetAllUpcomingEvents;
using IWema.Application.UpcomingEvents.Query.GetUpcomingEventById;
using IWema.Domain.Entity;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IWema.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpcomingEventsController(IMediator mediator) : BaseController
    {

        [Authorize(Roles = Role.ADMIN)]
        [HttpPost]
        public async Task<IActionResult> Add([FromForm] UpcomingEventsInputModel request)
        {
            var command = new AddUpcomingEventsCommand(request.File, request.NameOfEvent, request.Date);
            var response = await mediator.Send(command);
            return ServiceResponse(response);
        }

        [Authorize(Roles = Role.ADMIN)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await mediator.Send(new DeleteUpcomingEventsCommand(id));
            return ServiceResponse(response);
        }

        [Authorize(Roles = Role.ADMIN)]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var response = await mediator.Send(new GetAllUpcomingEventsQuery());
            return ServiceResponse(response);
        }

        [Authorize(Roles = Role.ADMIN)]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var response = await mediator.Send(new GetUpcomingEventsByIdQuery(id));
            return ServiceResponse(response);
        }

        [Authorize(Roles=Role.ADMIN)]
        [HttpPut("{id}")]
        public async Task<IActionResult> PartialUpdateUpcomingEvents(Guid id, [FromForm] PartiallyUpdateUpcomingEventsInputModel request)
        {
            PartiallyUpdateUpcomingEventsCommand command = new(request.Id, request.NameOfEvent, request.Date, request.File);
            var result = await mediator.Send(command);
            return ServiceResponse(result);
        }
    }

}
