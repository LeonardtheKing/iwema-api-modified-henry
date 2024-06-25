using IWema.Application.Common.DTO;
using IWema.Application.Contract;
using IWema.Domain.Entity;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace IWema.Application.UpcomingEvents.Command.Update;

public record UpdateUpcomingEventsCommand(Guid Id, string NameOfEvent, string Date, IFormFile File) : IRequest<ServiceResponse>;
public class UpdateUpcomingEventCommandHandler(IUpcomingEventsRepository upcomingEventsRepository, IFileHandler fileHandler) : IRequestHandler<UpdateUpcomingEventsCommand, ServiceResponse>
{
    public async Task<ServiceResponse> Handle(UpdateUpcomingEventsCommand command, CancellationToken cancellationToken)
    {
        // Retrieve the upcomingevent to update
        //UpcomingEventEntity announcement = await announcementRepository.GetById(command.Id);
        //if (announcement == null)
        //    return new("Upcoming Event not found.", false);

        //// Handle file update logic (if a file is provided)
        //if (announcement == null) return new(" Image Not Found", false);

        //var updatedImageLocation = await fileHandler.UpdateImage(command.File);

        //// Update management team member information
        //announcement.SetNameOfEvent(command.NameOfEvent);
        //announcement.SetDate(command.Date);
        //announcement.SetImageLocation(updatedImageLocation);

        //// Perform the update operation
        //var updated = await announcementRepository.UpdateAsync(command.Id, announcement);
        //if (updated == 0)
        //{
        //    return new("Upcoming Event  failed to update", false);
        //}
        //return new("Upcoming Event  was Updated Successfully", true);

        UpcomingEventEntity upcomingEvent = await upcomingEventsRepository.GetById(command.Id);
        if (upcomingEvent == null)
            return new("UpcomingEvent not found.", false);

        var updatedImageLocation = await fileHandler.UpdateImage(command.File);
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
