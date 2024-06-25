namespace IWema.Domain.Entity;

public class MenuBar(string name, string link, string description, string icon, long hits = 0) : EntityBase
{

    public string Name { get; private set; } = name;
    public string Link { get; private set; } = link;
    public string Description { get; private set; } = description;
    public string Icon { get; private set; } = icon;
    public long Hits { get; private set; } = hits;


    public void IncrementHits()
    {
        Hits++;
    }
}
