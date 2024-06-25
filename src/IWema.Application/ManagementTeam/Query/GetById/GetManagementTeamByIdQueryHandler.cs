using AutoMapper;
using IWema.Application.Common.DTO;
using IWema.Application.Contract;
using MediatR;

namespace IWema.Application.ManagementTeam.Query.GetById;

public record GetManagementTeamByIdQuery(Guid Id) : IRequest<ServiceResponse<GetAllManagementTeamByIdOutputModel>>;
public class GetManagementTeamByIdQueryHandler(IManagementTeamRepository managementTeamRepository) : IRequestHandler<GetManagementTeamByIdQuery, ServiceResponse<GetAllManagementTeamByIdOutputModel>>
{
    public async Task<ServiceResponse<GetAllManagementTeamByIdOutputModel>> Handle(GetManagementTeamByIdQuery query, CancellationToken cancellationToken)
    {
        var managementTeam = await managementTeamRepository.GetById(query.Id);
        if (managementTeam == null)
            return new("Record not found!");
        GetAllManagementTeamByIdOutputModel response = new(managementTeam.Id,managementTeam.NameOfExecutive,managementTeam.Position,managementTeam.Quote,managementTeam.ProfileLink,managementTeam.ImageName,managementTeam.ImageLocation);

        return new("", true, response);
    }
}


