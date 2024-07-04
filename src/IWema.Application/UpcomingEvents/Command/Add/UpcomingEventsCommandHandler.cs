﻿using IWema.Application.Common.DTO;
using IWema.Application.Common.Utilities;
using IWema.Application.Contract;
using IWema.Domain.Entity;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace IWema.Application.UpcomingEvents.Command.Add;

public record AddUpcomingEventsCommand(IFormFile File, string NameOfEvent, string Date) : IRequest<ServiceResponse>;

public class UpcomingEventsCommandHandler(IUpcomingEventsRepository announcementRepository,IHttpContextAccessor httpContextAccessor) : IRequestHandler<AddUpcomingEventsCommand, ServiceResponse>
{
    public async Task<ServiceResponse> Handle(AddUpcomingEventsCommand command, CancellationToken cancellationToken)
    {

        var response = await FileHandler.SaveFileAsync(command.File,cancellationToken);
        if (response == null)
            return new("Image upload failed");
        var imageUrl = await FileHandler.GetImageUrlAsync(command.File,httpContextAccessor);
        if (imageUrl == null || string.IsNullOrEmpty(imageUrl))
            return new("Image upload failed");


        var announcement = UpcomingEventEntity.Create(command.NameOfEvent, command.Date, imageUrl);

        var added = await announcementRepository.Add(announcement);

        return new(added ? "Created" : "Upcoming Event was not created.", added);
    }
}
