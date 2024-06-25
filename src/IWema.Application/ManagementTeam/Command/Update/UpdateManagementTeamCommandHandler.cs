using IWema.Application.Common.DTO;
using IWema.Application.Common.Utilities;
using IWema.Application.Contract;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace IWema.Application.ManagementTeam.Command.Update
{
    public record UpdateManagementTeamCommand(
        Guid Id,
        Guid ManagementTeamId, 
        IFormFile File,
        string ProfileLink,
        string NameOfExecutive,
        string Position,
        string Quote
    ) : IRequest<ServiceResponse>;

    public class UpdateManagementTeamCommandHandler(IManagementTeamRepository managementTeamRepository,IFileHandler fileHandler) : IRequestHandler<UpdateManagementTeamCommand, ServiceResponse>
    {
        public async Task<ServiceResponse> Handle(UpdateManagementTeamCommand command, CancellationToken cancellationToken)
        {
            // Retrieve the management team member to update
            Domain.Entity.ManagementTeamEntity managementTeam = await managementTeamRepository.GetById(command.Id);
            if (managementTeam == null)
                return new ("Management Team member not found.", false);

            // Handle file update logic (if a file is provided)
            if (managementTeam == null) return new("Management Team Image Not Found", false);
            
                var updatedImageLocation = await fileHandler.UpdateImage(command.File);
              
            // Update management team member information
            managementTeam.SetNameOfExecutive(command.NameOfExecutive);
            managementTeam.SetPosition(command.Position);
            managementTeam.SetQuote(command.Quote);
            managementTeam.SetProfileLink(command.ProfileLink);
            managementTeam.SetImageLocation(updatedImageLocation);
            managementTeam.SetUpdatedAtDate();

            // Perform the update operation
            var updated = await managementTeamRepository.UpdateAsync( command.Id,managementTeam);
           if(updated == 0)
            {
                return new("Management team was not successfully updated",false);
            }
           return new("Management Team was Updated Successfully",true);
        }
    }
}
