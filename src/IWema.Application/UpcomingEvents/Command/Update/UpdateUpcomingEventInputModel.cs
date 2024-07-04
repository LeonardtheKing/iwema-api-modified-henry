using Microsoft.AspNetCore.Http;

namespace IWema.Application.UpcomingEvents.Command.Update;

public record UpdateUpcomingEventInputModel
(
     Guid Id,
     IFormFile File ,
     string NameOfEvent,
     string Date
);
