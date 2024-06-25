namespace IWema.Domain.Entity;

public class ManagementTeamEntity : EntityBase
{
    public ManagementTeamEntity() { }

    public ManagementTeamEntity(string nameOfExecutive, string quote, string position, string profileLink, string imageName,string imageLocation)
    {
        NameOfExecutive = nameOfExecutive;
        Position = position;
        Quote = quote;
        ProfileLink = profileLink;
        ImageName = imageName;
        ImageLocation=imageLocation;
        CreatedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;
    }

    public string NameOfExecutive { get; private set; }
    public string Position { get; private set; } = string.Empty;
    public string Quote { get; private set; }
    public string ProfileLink { get; private set; }
    public string ImageName { get; private set; }
    public string ImageLocation { get; private set; }

    public void SetNameOfExecutive(string nameOfExecutive) => NameOfExecutive = nameOfExecutive;
    public void SetPosition(string position) => Position = position;
    public void SetQuote(string quote) => Quote = quote;
    public void SetProfileLink(string profileLink) => ProfileLink = profileLink;
    public void SetImageName(string imageName) => ImageName = imageName;
    public void SetImageLocation(string imageLocation) => ImageLocation = imageLocation;
    public void SetUpdatedAtDate() => UpdatedAt = DateTime.Now;
}


public class ManagementTeamImage : EntityBase
{
    public ManagementTeamImage() { }

    public ManagementTeamImage(string imageName, string imagelink)
    {
        ImageName = imageName;
        ImageLink = imagelink;
        CreatedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;
    }

    public string ImageName { get; private set; }
    public string ImageLink { get; private set; }

}