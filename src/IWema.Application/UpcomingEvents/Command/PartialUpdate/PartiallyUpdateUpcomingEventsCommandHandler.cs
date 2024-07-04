using IWema.Application.Common.DTO;
using IWema.Application.Common.Utilities;
using IWema.Application.Contract;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace IWema.Application.UpcomingEvents.Command.PartialUpdate;


public class PartiallyUpdateUpcomingEventsCommandHandler(IUpcomingEventsRepository upcomingEventsRepository,IHttpContextAccessor httpContextAccessor) : IRequestHandler<PartiallyUpdateUpcomingEventsCommand, ServiceResponse>
{
    public async Task<ServiceResponse> Handle(PartiallyUpdateUpcomingEventsCommand command, CancellationToken cancellationToken)
    {
        var upcomingEvent = await upcomingEventsRepository.GetById(command.Id);
        if (upcomingEvent == null)
            return new ("UpcomingEvent Team member not found.", false);

        if (command.File != null)
        {
            var updatedImageLocation = await FileHandler.UpdateImageAsync(command.File,httpContextAccessor);
            upcomingEvent.UpdateImageLocation(updatedImageLocation);
        }

        if (command.Date != null)
            upcomingEvent.UpdateDate(command.Date);

        if(command.NameOfEvent != null)
            upcomingEvent.UpdateNameOfEvent(command.NameOfEvent);


        var updated = await upcomingEventsRepository.UpdateAsync(command.Id, upcomingEvent);
        if (updated == 0)
            return new("Upcoming Event team was not successfully updated", false);

        return new("Upcoming Event Team was Updated Successfully", true);
    }
}
