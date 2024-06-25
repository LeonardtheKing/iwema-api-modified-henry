using IWema.Application.Common.DTO;
using IWema.Application.Contract;
using MediatR;

namespace IWema.Application.ManagementTeam.Command.Delete;

public record DeleteManagementTeamCommand(Guid Id) : IRequest<ServiceResponse>;
public class DeleteManagementTeamCommandHandler : IRequestHandler<DeleteManagementTeamCommand, ServiceResponse>
{
    private readonly IManagementTeamRepository _managementTeamRepository;
    public DeleteManagementTeamCommandHandler(IManagementTeamRepository managementTeamRepository)
    {
        _managementTeamRepository=managementTeamRepository;
    }
    public async  Task<ServiceResponse> Handle(DeleteManagementTeamCommand request, CancellationToken cancellationToken)
    {
        var managementTeam = await _managementTeamRepository.GetById(request.Id);
        if (managementTeam == null)
            return new("Mananagement Team member not found.");

        var delete = await _managementTeamRepository.Delete(request.Id);
        if (!delete)
            return new("An error occurred while deleting the Menu Bar.", false);
        return new("Mananagement Team member deleted successfully.", true);
    }
}
