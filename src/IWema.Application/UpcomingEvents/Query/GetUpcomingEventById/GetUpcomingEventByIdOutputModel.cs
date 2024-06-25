namespace IWema.Application.UpcomingEvents.Query.GetUpcomingEventById;

public record GetUpcomingEventByIdOutputModel
(
    Guid Id,
    string NameOfEvent,
    string Date,
    string ImageLocation
);

