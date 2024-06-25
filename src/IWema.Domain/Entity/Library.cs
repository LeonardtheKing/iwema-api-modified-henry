namespace IWema.Domain.Entity;

public class Library : EntityBase
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public string Type { get; private set; }
    public Library()
    {

    }

    private Library(string name, string description, string type)
    {
        Name = name;
        Description = description;
        Type = type;
    }

    public static Library Create(string name, string description, string type)
    {
        return new Library(name, description, type);
    }
}
