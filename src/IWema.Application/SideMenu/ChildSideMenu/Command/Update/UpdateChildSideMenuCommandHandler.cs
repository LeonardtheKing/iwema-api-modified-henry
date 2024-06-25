using IWema.Application.Common.DTO;
using IWema.Application.Contract;
using IWema.Domain.Entity;
using MediatR;

namespace IWema.Application.SideMenu.ChildSideMenu.Command.Update;

public record UpdateChildSideMenuCommand(Guid Id, Guid ParentSideMenuId, string Name, string Icon, string Link) : IRequest<ServiceResponse>;
public class UpdateChildSideMenuCommandHandler : IRequestHandler<UpdateChildSideMenuCommand, ServiceResponse>
{
    private readonly ISideMenuRepository _sidemenuRepository;
    public UpdateChildSideMenuCommandHandler(ISideMenuRepository sidemenuRepository)
    {
        _sidemenuRepository = sidemenuRepository;
    }
    public async Task<ServiceResponse> Handle(UpdateChildSideMenuCommand command, CancellationToken cancellationToken)
    {
        ChildSideMenuEntity childSideMenu = await _sidemenuRepository.GetChildSideMenuById(command.Id);
        if (childSideMenu == null)
            return new ServiceResponse("Child Toggle not found.", false);

        childSideMenu.Name = command.Name;
        childSideMenu.Icon = command.Icon;
        childSideMenu.Link = command.Link;
        childSideMenu.ParentSideMenuId = command.ParentSideMenuId;

        var updatedCount = await _sidemenuRepository.UpdateChildSideMenu(command.Id, childSideMenu);

        if (!updatedCount)
            return new ServiceResponse("Failed to update Child Toggle.", false);


        return new ServiceResponse("Child Toggle successfully updated.", true);

    }
}


