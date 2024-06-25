using IWema.Application.Common.DTO;
using IWema.Application.Contract;
using MediatR;

namespace IWema.Application.Announcements.Command.Delete;

public record DeleteAnnouncementCommand(Guid Id) : IRequest<ServiceResponse>;
public class DeleteAnnouncementCommandHandler(IAnnouncementRepository announcementRepository, IFileHandler fileHandler) : IRequestHandler<DeleteAnnouncementCommand, ServiceResponse>
{
    public async Task<ServiceResponse> Handle(DeleteAnnouncementCommand request, CancellationToken cancellationToken)
    {
        var announcement = await announcementRepository.GetSingleAnnouncement(request.Id);
        if (announcement == null)
            return new("Announcement Team not found.");

        var delete = await announcementRepository.DeleteAnnouncement(request.Id);
        if (delete == null)
            return new("An error occurred while deleting the announcement.", false);
        return new("Announcement deleted successfully.", true);
    }
}

