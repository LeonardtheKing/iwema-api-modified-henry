﻿using IWema.Application.Common.DTO;
using IWema.Application.Contract;
using MediatR;

namespace IWema.Application.Announcements.Command.PartialUpdate;


public class PartiallyUpdateAnnouncementCommandHandler(IAnnouncementRepository announcementRepository, IFileHandler fileHandler) : IRequestHandler<PartiallyUpdateAnnouncementCommand, ServiceResponse>
{
    public async Task<ServiceResponse> Handle(PartiallyUpdateAnnouncementCommand command, CancellationToken cancellationToken)
    {
        var announcement = await announcementRepository.GetById(command.Id);
        if (announcement == null)
            return new("Announcement  not found.", false);

        if (command.File != null)
        {
            var updatedImageLocation = await fileHandler.UpdateImage(command.File);
            announcement.UpdateImageLocation(updatedImageLocation);
        }

        if (command.Title != null)
            announcement.UpdateTitle(command.Title);

        if (command.Date != null)
            announcement.UpdateDate(command.Date);

        if (command.Content != null)
            announcement.UpdateDate(command.Content);

        if (command.Link != null)
            announcement.UpdateLink(command.Link);


        var updated = await announcementRepository.UpdateAsync(command.Id, announcement);
        if (updated == 0)
            return new("Announcement was not successfully updated", false);

        return new("Announcement was Updated Successfully", true);
    }
}