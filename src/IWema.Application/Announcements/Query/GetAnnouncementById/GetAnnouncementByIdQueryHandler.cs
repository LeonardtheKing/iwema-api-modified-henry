using AutoMapper;
using IWema.Application.Common.DTO;
using IWema.Application.Contract;
using MediatR;

namespace IWema.Application.Announcements.Query.GetAnnouncementById;

public record GetAnnouncementByIdQuery(Guid Id) : IRequest<ServiceResponse<GetAnnouncementByIdOutputModel>>;
public class GetAnnouncementByIdQueryHandler(IAnnouncementRepository announcementRepository, IMapper mapper) : IRequestHandler<GetAnnouncementByIdQuery, ServiceResponse<GetAnnouncementByIdOutputModel>>
{
    public async Task<ServiceResponse<GetAnnouncementByIdOutputModel>> Handle(GetAnnouncementByIdQuery request, CancellationToken cancellationToken)
    {
        var announcement = await announcementRepository.GetSingleAnnouncement(request.Id);
        if (announcement == null)
            return new("Record not found!");
        var result = mapper.Map<GetAnnouncementByIdOutputModel>(announcement); ;

        return new("", true, result);
    }
}
