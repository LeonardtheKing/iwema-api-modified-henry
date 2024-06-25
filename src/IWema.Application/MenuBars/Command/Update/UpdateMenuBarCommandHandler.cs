using AutoMapper;
using IWema.Application.Common.DTO;
using IWema.Application.Contract;
using IWema.Domain.Entity;
using MediatR;

namespace IWema.Application.MenuBars.Command.Update;

public record UpdateMenuBarCommand(Guid Id, Guid MenuId, string Name, string Link, string Description, string Icon) : IRequest<ServiceResponse>;

public class UpdateMenuBarCommandHandler(IMenuBarRepository menuBarRepository, IMapper mapper) : IRequestHandler<UpdateMenuBarCommand, ServiceResponse>
{
    public async Task<ServiceResponse> Handle(UpdateMenuBarCommand request, CancellationToken cancellationToken)
    {
        // Retrieve the existing MenuBar
        MenuBar menuBar = await menuBarRepository.GetById(request.MenuId);
        // If the MenuBar does not exist, return an error
        if (menuBar == null)
            return new("Menu Bar not found.", false);
        // Map the updated properties from the request to the existing MenuBar
        mapper.Map(request, menuBar);
        // Persist the changes using the repository
        await menuBarRepository.Update(menuBar);
        // Return the affected rows (typically 1 for successful update)
        return new("Menu Bar Updated Successfully.", true);
    }
}