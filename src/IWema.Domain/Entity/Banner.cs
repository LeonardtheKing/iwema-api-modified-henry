namespace IWema.Domain.Entity;

public class Banner : EntityBase
{
    public string Name { get; private set; }
    public string Title { get; private set; }
    public bool IsActive { get; private set; }
    public Banner() { }
    private Banner(string name, string title, bool isAtive)
    {
        Name = name;
        Title = title;
        IsActive = isAtive;
        CreatedAt = DateTime.Now;
        UpdatedAt = CreatedAt;
    }

    public static Banner Create(string name, string title, bool isActive)
    {
        return new Banner(name, title, isActive);
    }

    public void Update(string name, string title, bool isActive)
    {
        Name = name;
        Title = title;
        IsActive = isActive;
        UpdatedAt = DateTime.Now;
    }
}