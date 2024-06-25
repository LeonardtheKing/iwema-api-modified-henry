namespace IWema.Domain.Entity;

public class BlogEntity : EntityBase
{
    public string ImageLocation { get; private set; }
    public string Title { get; private set; }
    public string Summary { get; private set; }
    public string ReadMoreLink { get; private set; }

    public BlogEntity() { }

    public BlogEntity(string imageLocation, string title, string summary, string readMoreLink)
    {
        ImageLocation = imageLocation;
        Title = title;
        Summary = summary;
        ReadMoreLink = readMoreLink;
        CreatedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;
    }



    public static BlogEntity Create(string imageLocation, string title, string summary, string readMoreLink)
    {
        return new BlogEntity( imageLocation,  title,  summary,  readMoreLink);
    }

    public void Update(string imageLocation, string title, string summary, string readMoreLink)
    {
        ImageLocation = imageLocation;
        Title = title;
        Summary = summary;
        ReadMoreLink = readMoreLink;
    }

    public void UpdateImageLocation(string imageLocation)
    {
        ImageLocation = imageLocation;
    }

    public void UpdateTitle(string title)
    {
        Title = title;
    }

    public void UpdateSummary(string summary)
    {
        Summary = summary;
    }

    public void UpdateReadMoreLink(string readMoreLink)
    {
        ReadMoreLink = readMoreLink;
    }

}

