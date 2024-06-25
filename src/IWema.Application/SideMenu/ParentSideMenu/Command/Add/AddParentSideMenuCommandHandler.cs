using IWema.Application.Common.DTO;
using IWema.Application.Contract;
using IWema.Domain.Entity;
using MediatR;

namespace IWema.Application.SideMenu.ParentSideMenu.Command.Add;

public record AddParentSideMenuCommand(string Name, string Icon) : IRequest<ServiceResponse>;
public class AddParentSideMenuCommandHandler(ISideMenuRepository sideMenuRepository) : IRequestHandler<AddParentSideMenuCommand, ServiceResponse>
{
    public async Task<ServiceResponse> Handle(AddParentSideMenuCommand command, CancellationToken cancellationToken)
    {
        ParentSideMenuEntity parentSideMenu = new(command.Name, command.Icon);

        var added = await sideMenuRepository.Add(parentSideMenu);
        if (!added)
            return new("Failed to add parent side Menu.");

        return new("Parent  side menu successfully added.", true);
    }
}
