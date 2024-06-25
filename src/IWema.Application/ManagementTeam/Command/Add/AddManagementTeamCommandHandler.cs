using IWema.Application.Common.DTO;
using IWema.Application.Common.Utilities;
using IWema.Application.Contract;
using IWema.Domain.Entity;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace IWema.Application.ManagementTeam.Command.Add;

public record AddManagementTeamCommand(IFormFile File, string nameOfExecutive,string position,string quote,string profileLink) : IRequest<ServiceResponse>;
public class AddManagementTeamCommandHandler(IManagementTeamRepository managementTeamRepository,IFileHandler fileHandler) : IRequestHandler<AddManagementTeamCommand, ServiceResponse>
{
    public async Task<ServiceResponse> Handle(AddManagementTeamCommand command, CancellationToken cancellationToken)
    {
        var response = await fileHandler.SaveFile(command.File);
        if (response == null)
            return new("Image upload failed");
        var imageUrl = await fileHandler.GetImageUrl(command.File);
        if(imageUrl == null || string.IsNullOrEmpty(imageUrl))
            return new("Image upload failed"); 

        ManagementTeamEntity managementTeam = new(command.nameOfExecutive,command.position,command.quote,command.profileLink,response.Response, imageUrl);

        var added = await managementTeamRepository.Add(managementTeam);

        return new(added ? "Created" : "Management Team upload failed.", added);
    }
}
