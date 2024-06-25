using IWema.Application.Common.DTO;
using IWema.Application.Contract;
using MediatR;

namespace IWema.Application.MenuBars.Command.Delete;

public record DeleteMenuBarCommand(Guid Id) : IRequest<ServiceResponse>;

public class DeleteMenuBarCommandHandler(IMenuBarRepository menuBarRepository) : IRequestHandler<DeleteMenuBarCommand, ServiceResponse>
{
    public async Task<ServiceResponse> Handle(DeleteMenuBarCommand request, CancellationToken cancellationToken)
    {
        var menu = await menuBarRepository.GetById(request.Id);
        if (menu == null)
            return new("Menu Bar not found.");

     var delete =  await menuBarRepository.Delete(request.Id);
        if (!delete)
            return new("An error occurred while deleting the Menu Bar.",false);
        return new("Menu Bar deleted successfully.", true);
    }
}