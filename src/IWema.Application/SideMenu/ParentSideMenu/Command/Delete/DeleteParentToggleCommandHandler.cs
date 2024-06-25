using IWema.Application.Common.DTO;
using IWema.Application.Contract;
using MediatR;

namespace IWema.Application.SideMenu.ParentSideMenu.Command.Delete;

public record DeleteParentToggleCommand(Guid Id) : IRequest<ServiceResponse>;
public class DeleteParentToggleCommandHandler(ISideMenuRepository sideMenuRepository) : IRequestHandler<DeleteParentToggleCommand, ServiceResponse>
{
    public async Task<ServiceResponse> Handle(DeleteParentToggleCommand command, CancellationToken cancellationToken)
    {
        var parentToggle = await sideMenuRepository.GetParentSideMenuById(command.Id);
        if (parentToggle == null)
            return new("parentToggle not found.");

        var delete = await sideMenuRepository.DeleteParentSideMenu(command.Id);
        if (!delete)
            return new("An error occured while deleting the parent toggle");
        return new("parentToggle deleted successfully.", true);
    }
}

