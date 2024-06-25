using AutoMapper;
using IWema.Application.Announcements.Query.GetAllAnnouncements;
using IWema.Application.Common.DTO;
using IWema.Application.Contract;
using MediatR;

public record GetAllAnnouncementQuery : IRequest<ServiceResponse<List<GetAllAnnouncementsOutputModel>>>;

public class GetAllAnnouncementQueryHandler(IMapper mapper, IAnnouncementRepository announcementRepository) : IRequestHandler<GetAllAnnouncementQuery, ServiceResponse<List<GetAllAnnouncementsOutputModel>>>
{
    public async Task<ServiceResponse<List<GetAllAnnouncementsOutputModel>>> Handle(GetAllAnnouncementQuery request, CancellationToken cancellationToken)
    {
        var announcements = await announcementRepository.GetAllAnnouncements();

        // Use AutoMapper to map the announcements to the output model
        var result = mapper.Map<List<GetAllAnnouncementsOutputModel>>(announcements);

        return new("", true, result);
    }
}

