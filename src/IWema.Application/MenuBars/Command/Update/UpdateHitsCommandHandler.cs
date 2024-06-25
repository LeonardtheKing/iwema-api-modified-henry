using AutoMapper;
using IWema.Application.Common.DTO;
using IWema.Application.Contract;
using MediatR;

namespace IWema.Application.MenuBars.Command.Update;

public record UpdateHitsCommand(Guid Id) : IRequest<ServiceResponse>;

public class UpdateHitsCommandHandler(IMenuBarRepository menuBarRepository) : IRequestHandler<UpdateHitsCommand, ServiceResponse>
{
    public async Task<ServiceResponse> Handle(UpdateHitsCommand request, CancellationToken cancellationToken)
    {
        var menuBar = await menuBarRepository.GetById(request.Id);
        menuBar.IncrementHits();
        // Save the updated MenuBar
        await menuBarRepository.Update(menuBar);

        return new("Hits Updated Successfully.", true);
    }
}
