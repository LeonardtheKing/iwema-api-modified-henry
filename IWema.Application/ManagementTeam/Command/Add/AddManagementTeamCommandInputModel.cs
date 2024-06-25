using Microsoft.AspNetCore.Http;

namespace IWema.Application.ManagementTeam.Command.Add;

public class AddManagementTeamCommandInputModel
{
    public IFormFile File { get; set; }
    public string ImageLink { get; set; } = string.Empty;
    public string nameOfExecutive { get; set; } = string.Empty;
    public string position { get; set; } = string.Empty;
    public string quote { get; set; } = string.Empty;
    public string profileLink { get; set; } = string.Empty;
}
