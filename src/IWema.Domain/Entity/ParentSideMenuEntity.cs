using System.ComponentModel.DataAnnotations.Schema;

namespace IWema.Domain.Entity;
public class ParentSideMenuEntity : EntityBase
{
    public ParentSideMenuEntity() { } // Parameterless constructor

    public ParentSideMenuEntity(string name, string icon)
    {
        Name = name;
        Icon = icon;
    }
    public string Name { get;private set; }
    public string Icon { get; private set; }

}



