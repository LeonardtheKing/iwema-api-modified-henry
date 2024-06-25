using Microsoft.AspNetCore.Http;

namespace IWema.Application.UpcomingEvents.Command.Add;

public class UpcomingEventsInputModel
{
    public IFormFile File { get; set; }
    public string NameOfEvent { get; set; } = string.Empty;
    public string Date { get; set; } = string.Empty;

}
