namespace IWema.Application.Announcements.Query.GetAllAnnouncements;


public class GetAllAnnouncementsOutputModel
{
    public Guid AnnouncementId { get; set; }
    public string Title { get; set; }=string.Empty;
    public string Date { get; set; }= string.Empty;
    public string ImageLocation { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string Link { get; set; } = string.Empty;
}

