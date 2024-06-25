using IWema.Application.Common.DTO;
using IWema.Application.Contract;
using IWema.Domain.Entity;
using MediatR;

namespace IWema.Application.SideMenu.ChildSideMenu.Command.Add;

public record AddChildSideMenuCommand(Guid ParentToggleId, string Name, string Link, string Icon) : IRequest<ServiceResponse>;
public class AddChildSideMenuCommandHandler(ISideMenuRepository sideMenuRepository) : IRequestHandler<AddChildSideMenuCommand, ServiceResponse>
{
    public async Task<ServiceResponse> Handle(AddChildSideMenuCommand command, CancellationToken cancellationToken)
    {
        ChildSideMenuEntity subMenu = new(command.Name, command.Link, command.Icon, command.ParentToggleId);
        if (subMenu.ParentSideMenuId == Guid.Empty)
            return new("Invalid ParentSideMenuId.", false);
        // Check if the parent menu exists
        var parentMenu = await sideMenuRepository.GetParentSideMenuById(subMenu.ParentSideMenuId);
        if (parentMenu == null)
            return new("Parent menu does not exist.", false);

        var added = await sideMenuRepository.AddSubMenu(subMenu);
        if (!added)
            return new("Failed to add Child Side MenuEntity.");

        return new("Child Side MenuEntity successfully added.", true);
    }
}
