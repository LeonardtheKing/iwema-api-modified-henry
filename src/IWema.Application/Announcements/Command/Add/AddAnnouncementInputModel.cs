using Microsoft.AspNetCore.Http;

namespace IWema.Application.Announcements.Command.Add;

public class AddAnnouncementInputModel
{
    public IFormFile File { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Date { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? Link { get; set; } = null;

}
