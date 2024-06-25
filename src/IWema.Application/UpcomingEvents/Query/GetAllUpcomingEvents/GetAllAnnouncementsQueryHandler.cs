using IWema.Application.Common.DTO;
using IWema.Application.Contract;
using MediatR;

namespace IWema.Application.UpcomingEvents.Query.GetAllUpcomingEvents;

public record GetAllUpcomingEventsQuery : IRequest<ServiceResponse<List<GetAllUpcomingEventsOutputModel>>>;
public class GetAllUpcomingEventsQueryHandler(IUpcomingEventsRepository announcementRepository) : IRequestHandler<GetAllUpcomingEventsQuery, ServiceResponse<List<GetAllUpcomingEventsOutputModel>>>
{
    public async Task<ServiceResponse<List<GetAllUpcomingEventsOutputModel>>> Handle(GetAllUpcomingEventsQuery request, CancellationToken cancellationToken)
    {
        var announcements = await announcementRepository.Get();
        List<GetAllUpcomingEventsOutputModel> result = announcements.Select(x => new GetAllUpcomingEventsOutputModel(x.Id, x.NameOfEvent, x.Date, x.ImageLocation)).ToList();

        return new("", true, result);
    }
}
