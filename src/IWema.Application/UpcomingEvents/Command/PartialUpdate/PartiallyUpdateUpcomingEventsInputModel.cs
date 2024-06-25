using Microsoft.AspNetCore.Http;

namespace IWema.Application.UpcomingEvents.Command.PartialUpdate;

public record PartiallyUpdateUpcomingEventsInputModel
(
 Guid Id,
 string? NameOfEvent = null,
 string? Date = null,
 IFormFile? File = null
 );
