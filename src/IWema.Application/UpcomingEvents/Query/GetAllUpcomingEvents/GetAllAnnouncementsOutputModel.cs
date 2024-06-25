namespace IWema.Application.UpcomingEvents.Query.GetAllUpcomingEvents;

public record GetAllUpcomingEventsOutputModel
(
    Guid Id,
    string NameOfEvent,
    string Date,
    string ImageLocation
);
