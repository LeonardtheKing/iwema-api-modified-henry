using AutoMapper;
using IWema.Application.Common.DTO;
using IWema.Application.Contract;
using IWema.Application.MenuBars.Query.GetAll;
using IWema.Domain.Entity;
using MediatR;

namespace IWema.Application.ManagementTeam.Query.GetAll;

public record GetAllManageentTeamQuery : IRequest<ServiceResponse<List<GetAllManagementTeamOutputModel>>>;
public class GetAllManageentTeamQueryHandler(IManagementTeamRepository managementTeamRepository) : IRequestHandler<GetAllManageentTeamQuery, ServiceResponse<List<GetAllManagementTeamOutputModel>>>
{
    public async Task<ServiceResponse<List<GetAllManagementTeamOutputModel>>> Handle(GetAllManageentTeamQuery query, CancellationToken cancellationToken)
    {
        var managementTeams = await managementTeamRepository.Get();
        List<GetAllManagementTeamOutputModel> result = managementTeams.Select(x => new GetAllManagementTeamOutputModel(x.Id, x.NameOfExecutive, x.Position, x.Quote, x.ProfileLink,x.ImageName,x.ImageLocation)).ToList();
       // var result = mapper.Map<List<GetAllManagementTeamOutputModel>>(managementTeams);

        return new("", true, result);
    }
}
