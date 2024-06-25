using AutoMapper;
using IWema.Application.Common.DTO;
using IWema.Application.Contract;
using IWema.Domain.Entity;
using MediatR;

namespace IWema.Application.MenuBars.Command.Add;

public record AddMenuBarCommand(string Name, string Link, string Description, string Icon, long Hits) : IRequest<ServiceResponse>;

public class AddMenuBarCommandHandler(IMenuBarRepository menuBarRepository) : IRequestHandler<AddMenuBarCommand, ServiceResponse>
{
    public async Task<ServiceResponse> Handle(AddMenuBarCommand command, CancellationToken cancellationToken)
    {
        MenuBar menuBar = new(command.Name, command.Link, command.Description, command.Icon, command.Hits);

        var added = await menuBarRepository.Add(menuBar);
        if (!added)
            return new("Failed to add menu.");

        return new("menu successfully added.", true);
    }
}
