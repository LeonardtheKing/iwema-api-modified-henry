using IWema.Application.Common.DTO;
using IWema.Application.Contract;
using MediatR;

namespace IWema.Application.UpcomingEvents.Query.GetUpcomingEventById;

public record GetUpcomingEventsByIdQuery(Guid Id) : IRequest<ServiceResponse<GetUpcomingEventByIdOutputModel>>;
public class GetUpcomingEventsByIdQueryHandler(IUpcomingEventsRepository upcomingEventsRepository) : IRequestHandler<GetUpcomingEventsByIdQuery, ServiceResponse<GetUpcomingEventByIdOutputModel>>
{
    public async Task<ServiceResponse<GetUpcomingEventByIdOutputModel>> Handle(GetUpcomingEventsByIdQuery query, CancellationToken cancellationToken)
    {
        var upcomingEvents = await upcomingEventsRepository.GetById(query.Id);
        if (upcomingEvents == null)
            return new("Record not found!");
        GetUpcomingEventByIdOutputModel response = new(upcomingEvents.Id, upcomingEvents.NameOfEvent, upcomingEvents.Date, upcomingEvents.ImageLocation);

        return new("", true, response);
    }
}
