using IWema.Application.Common.DTO;
using IWema.Application.Contract;
using MediatR;

namespace IWema.Application.UpcomingEvents.Command.PartialUpdate;


public class PartiallyUpdateUpcomingEventsCommandHandler(IUpcomingEventsRepository upcomingEventsRepository, IFileHandler fileHandler) : IRequestHandler<PartiallyUpdateUpcomingEventsCommand, ServiceResponse>
{
    public async Task<ServiceResponse> Handle(PartiallyUpdateUpcomingEventsCommand command, CancellationToken cancellationToken)
    {
        var upcomingEvent = await upcomingEventsRepository.GetById(command.Id);
        if (upcomingEvent == null)
            return new ("UpcomingEvent Team member not found.", false);

        if (command.File != null)
        {
            var updatedImageLocation = await fileHandler.UpdateImage(command.File);
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
