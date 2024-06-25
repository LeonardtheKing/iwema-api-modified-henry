using AutoMapper;
using IWema.Application.Common.DTO;
using IWema.Application.Contract;
using MediatR;

namespace IWema.Application.ManagementTeam.Query.GetById;

public record GetManagementTeamByIdQuery(Guid Id) : IRequest<ServiceResponse<GetAllManagementTeamByIdOutputModel>>;
public class GetManagementTeamByIdQueryHandler(IManagementTeamRepository managementTeamRepository, IMapper mapper) : IRequestHandler<GetManagementTeamByIdQuery, ServiceResponse<GetAllManagementTeamByIdOutputModel>>
{
    public async Task<ServiceResponse<GetAllManagementTeamByIdOutputModel>> Handle(GetManagementTeamByIdQuery query, CancellationToken cancellationToken)
    {
        var managementTeam = await managementTeamRepository.GetById(query.Id);
        if (managementTeam == null)
            return new("Record not found!");
        GetAllManagementTeamByIdOutputModel response = new(managementTeam.Id,managementTeam.Quote,managementTeam.ProfileLink,managementTeam.ImageLink,managementTeam.ImageLocation,managementTeam.ImageName,managementTeam.NameOfExecutive,managementTeam.Position);

        return new("", true, response);
    }
}


