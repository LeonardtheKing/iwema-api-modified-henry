using AutoMapper;
using IWema.Application.Common.DTO;
using IWema.Application.Contract;
using IWema.Domain.Entity;
using MediatR;

namespace IWema.Application.SideMenu.ParentSideMenu.Command.Update;

public record UpdateParentSideMenuCommand(Guid Id, Guid ParentSideMenuId, string Name, string Icon) : IRequest<ServiceResponse>;
public class UpdateParentSideMenuCommandHandler(ISideMenuRepository sideMenuRepository) : IRequestHandler<UpdateParentSideMenuCommand, ServiceResponse>
{
    public async Task<ServiceResponse> Handle(UpdateParentSideMenuCommand command, CancellationToken cancellationToken)
    {
        ParentSideMenuEntity parentSideMenu = await sideMenuRepository.GetParentSideMenuById(command.Id);
        if (parentSideMenu == null)
            return new("Parent Side Menu not found.", false);

        // mapper.Map(command, parentToggle);
        ParentSideMenuEntity updatedParentSideMenuEntity = new(command.Icon, command.Name);
        await sideMenuRepository.UpdateParentSideMenu(updatedParentSideMenuEntity);

        return new("Parent Side Menu successfully updated.", true);
    }

}





