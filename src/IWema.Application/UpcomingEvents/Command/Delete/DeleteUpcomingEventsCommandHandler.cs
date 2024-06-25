using IWema.Application.Common.DTO;
using IWema.Application.Contract;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace IWema.Application.UpcomingEvents.Command.Delete;
public record DeleteUpcomingEventsCommand(Guid Id) : IRequest<ServiceResponse>;
public class DeleteUpcomingEventsCommandHandler(IUpcomingEventsRepository announcementRepository) : IRequestHandler<DeleteUpcomingEventsCommand, ServiceResponse>
{
    public async Task<ServiceResponse> Handle(DeleteUpcomingEventsCommand command, CancellationToken cancellationToken)
    {
        var upcomingEvent = await announcementRepository.GetById(command.Id);
        if (upcomingEvent == null)
            return new("Upcoming Event  not found.");

        var delete = await announcementRepository.Delete(command.Id);
        if (!delete)
            return new("An error occurred while deleting the Announcement.", false);
        return new("Event  deleted successfully.", true);
    }
}
