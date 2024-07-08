using IWema.Application.Common.DTO;
using IWema.Application.Common.Utilities;
using IWema.Application.Contract;
using IWema.Domain.Entity;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace IWema.Application.UpcomingEvents.Command.Update;

public record UpdateUpcomingEventsCommand(Guid Id, string NameOfEvent, string Date, IFormFile File) : IRequest<ServiceResponse>;
public class UpdateUpcomingEventCommandHandler(IUpcomingEventsRepository upcomingEventsRepository,IHttpContextAccessor httpContextAccessor, IWebHostEnvironment env) : IRequestHandler<UpdateUpcomingEventsCommand, ServiceResponse>
{
    public async Task<ServiceResponse> Handle(UpdateUpcomingEventsCommand command, CancellationToken cancellationToken)
    {
       
        UpcomingEventEntity upcomingEvent = await upcomingEventsRepository.GetById(command.Id);
        if (upcomingEvent == null)
            return new("UpcomingEvent not found.", false);

        var updatedImageLocation
            = await FileHandler.UpdateImageAsync(command.File,httpContextAccessor,env);
        if (updatedImageLocation == null || string.IsNullOrEmpty(updatedImageLocation)) return new("file not found");

        // Update Upcoming Event information
        upcomingEvent.Update(command.NameOfEvent, command.Date, updatedImageLocation);

        // Perform the update operation
        var updated = await upcomingEventsRepository.UpdateAsync(command.Id, upcomingEvent);
        if (updated == 0)
        {
            return new("UpcomingEvent was not successfully updated", false);
        }
        return new("UpcomingEvent was Updated Successfully", true);
    }
}
