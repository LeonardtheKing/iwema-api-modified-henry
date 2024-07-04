namespace IWema.Domain.Entity;

public class AnnouncementDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Date { get; set; } = string.Empty;
    public string ImageLocation { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? Link { get; set; } = null;
}
