using IWema.Application.Common.DTO;
using IWema.Application.Common.Utilities;
using IWema.Application.Contract;
using IWema.Domain.Entity;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace IWema.Application.ManagementTeam.Command.Add;

public record AddManagementTeamCommand(IFormFile File, string nameOfExecutive,string position,string quote,string profileLink) : IRequest<ServiceResponse>;
public class AddManagementTeamCommandHandler(IManagementTeamRepository managementTeamRepository,IHttpContextAccessor httpContextAccessor, IWebHostEnvironment env) : IRequestHandler<AddManagementTeamCommand, ServiceResponse>
{
    public async Task<ServiceResponse> Handle(AddManagementTeamCommand command, CancellationToken cancellationToken)
    {
        var response = await FileHandler.SaveFileAsync(command.File,env,cancellationToken);
        if (response == null)
            return new("Image upload failed");
        var imageUrl = await FileHandler.GetImageUrlAsync(command.File,httpContextAccessor,env);
        if(imageUrl == null || string.IsNullOrEmpty(imageUrl))
            return new("Image upload failed"); 

        ManagementTeamEntity managementTeam = new(command.nameOfExecutive,command.position,command.quote,command.profileLink,response, imageUrl);

        var added = await managementTeamRepository.Add(managementTeam);

        return new(added ? "Created" : "Management Team upload failed.", added);
    }
}
