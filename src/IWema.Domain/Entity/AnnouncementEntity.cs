namespace IWema.Domain.Entity;

public class AnnouncementEntity : EntityBase
{
    public string Title { get; private set; }
    public string Date { get; private set; }
    public string ImageLocation { get; private set; }
    public string Content { get; private set; }
    public string? Link { get; private set; }
    public AnnouncementEntity() { }
    public AnnouncementEntity(string title, string date, string imageLocation, string content, string link)
    {
        Title = title;
        Date = date;
        ImageLocation = imageLocation;
        Content = content;
        Link = link;

    }

    public static AnnouncementEntity Create(string title, string date, string imageLocation, string content, string link)
    {
        if (string.IsNullOrEmpty(link))
        {
            return CreateWithoutLink(title, date, imageLocation, content);
        }

        return new AnnouncementEntity(title, date, imageLocation, content, link);
    }

    public static AnnouncementEntity CreateWithoutLink(string title, string date, string imageLocation, string content)
    {
        return new AnnouncementEntity(title, date, imageLocation, content, null);
    }

    public void Update(string title, string date, string imageLocation, string content, string link)
    {
        Title = title;
        Date = date;
        ImageLocation = imageLocation;
        Content = content;
        Link = link;
    }

    public void UpdateTitle(string title)
    {
        Title = title;
    }

    public void UpdateDate(string date)
    {
        Date = date;
    }

    public void UpdateImageLocation(string imageLocation)
    {
        ImageLocation = imageLocation;

    }

    public void UpdateContent(string content)
    {

        Content = content;
    }

    public void UpdateLink(string link)
    {
        Link = link;
    }

}


