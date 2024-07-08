using IWema.Application.Common.DTO;
using IWema.Application.Common.Utilities;
using IWema.Application.Contract;
using IWema.Domain.Entity;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace IWema.Application.Announcements.Command.Update;

public record UpdateAnnouncementCommand(
  Guid Id,
    IFormFile File,
    string Title,
    string Date,
    string Content,
    string Link
) : IRequest<ServiceResponse>;
public class UpdateAnnouncementCommandHandler(IAnnouncementRepository announcementRepository,IHttpContextAccessor httpContextAccessor, IWebHostEnvironment env) : IRequestHandler<UpdateAnnouncementCommand, ServiceResponse>
{
    public async Task<ServiceResponse> Handle(UpdateAnnouncementCommand command, CancellationToken cancellationToken)
    {
        // Retrieve the management team member to update
        AnnouncementEntity announcement = await announcementRepository.GetById(command.Id);
        if (announcement == null)
            return new("Announcement not found.", false);

        // Handle file update logic (if a file is provided)
        if (announcement == null) return new("Announcement Not Found", false);

        var updatedImageLocation = await FileHandler.UpdateImageAsync(command.File,httpContextAccessor,env);

        // Update management team member information
        announcement.Update(command.Title, command.Date, updatedImageLocation, command.Content, command.Link);

        // Perform the update operation
        var updated = await announcementRepository.UpdateAsync(command.Id, announcement);
        if (updated == 0)
        {
            return new("Announcement was not successfully updated", false);
        }
        return new("Announcement was Updated Successfully", true);
    }
}
