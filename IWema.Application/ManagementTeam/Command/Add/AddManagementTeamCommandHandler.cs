using IWema.Application.Common.DTO;
using IWema.Application.Common.Utilities;
using IWema.Application.Contract;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace IWema.Application.ManagementTeam.Command.Add;

public record AddManagementTeamCommand(IFormFile File, string imageLink,string nameOfExecutive,string position,string quote,string profileLink) : IRequest<ServiceResponse>;
public class AddManagementTeamCommandHandler(IManagementTeamRepository managementTeamRepository) : IRequestHandler<AddManagementTeamCommand, ServiceResponse>
{
    public async Task<ServiceResponse> Handle(AddManagementTeamCommand command, CancellationToken cancellationToken)
    {
        var response = await FileHandler.SaveFile(command.File);
        var imageUrl = await FileHandler.GetImageUrl(command.File);

        if (!response.Successful)
            return response;
        if(!imageUrl.Successful)
            return imageUrl;

        Domain.Entity.ManagementTeam managementTeam = new(command.nameOfExecutive,command.position,command.quote,command.profileLink,response.Response,command.imageLink, imageUrl.Response);

        var added = await managementTeamRepository.Add(managementTeam);

        return new(added ? "Created" : "Management Team upload failed.", added);
    }
}
