using IWema.Application.Common.DTO;
using IWema.Application.Common.Utilities;
using IWema.Application.Contract;
using IWema.Domain.Entity;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace IWema.Application.Announcements.Command.Add;

public record AddAnnouncementCommand(IFormFile File, string Title, string Date, string Content, string? Link) : IRequest<ServiceResponse>;
public class AddAnnouncementCommandHandler(IAnnouncementRepository announcementRepository, IHttpContextAccessor httpContextAccessor) : IRequestHandler<AddAnnouncementCommand, ServiceResponse>
{
    public async Task<ServiceResponse> Handle(AddAnnouncementCommand command, CancellationToken cancellationToken)
    {
        var saveFile = await FileHandler.SaveFileAsync(command.File,cancellationToken);
        if (saveFile == null)
            return new ("Image upload failed");

        var imageUrl = await FileHandler.GetImageUrlAsync(command.File,httpContextAccessor);
        if (imageUrl == null)
            return new("Image upload failed");


        AnnouncementEntity announcement;
        if (string.IsNullOrEmpty(command.Link))
        {
            announcement = AnnouncementEntity.CreateWithoutLink(command.Title, command.Date, imageUrl, command.Content);
        }
        else
        {
            announcement = AnnouncementEntity.Create(command.Title, command.Date, imageUrl, command.Content, command.Link);
        }

        var added = await announcementRepository.Add(announcement);

        return new(added ? "Created" : "Announcement upload failed.", added);
    }
}
