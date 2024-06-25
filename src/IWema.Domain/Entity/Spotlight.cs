namespace IWema.Domain.Entity;

public class Spotlight : EntityBase
{
    public string Title { get; private set; }
    public string Content { get; private set; }
    public string Url { get; private set; }
    public bool IsActive { get; private set; }
    public Spotlight()
    {
        
    }
    public Spotlight(string title, string content, string url, bool isAtive = true)
    {
        Title = title;
        Content = content;
        Url = url;
        IsActive = isAtive;
        CreatedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;
    }

    public void Update(string title, string content, string url, bool isActive)
    {
        Title = title;
        Content = content;
        Url = url;
        IsActive = isActive;
        UpdatedAt = DateTime.Now;
    }
}
