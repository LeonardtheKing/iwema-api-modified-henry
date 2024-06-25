using IWema.Application.Common.DTO;
using IWema.Application.Contract;
using MediatR;

namespace IWema.Application.SideMenu.ChildSideMenu.Command.Delete;

public record DeleteChildToggleCommand(Guid Id) : IRequest<ServiceResponse>;
public class DeleteChildToggleCommandHandler(ISideMenuRepository sidemenuRepository) : IRequestHandler<DeleteChildToggleCommand, ServiceResponse>
{
    public async Task<ServiceResponse> Handle(DeleteChildToggleCommand command, CancellationToken cancellationToken)
    {
        var childToggle = await sidemenuRepository.GetChildSideMenuById(command.Id);
        if (childToggle == null)
            return new("childToggle not found.");

        var delete = await sidemenuRepository.DeleteChildSideMenu(command.Id);
        if (!delete)
            return new("An error occured while deleting the child toggle");
        return new("childToggle deleted successfully.", true);
    }
}
