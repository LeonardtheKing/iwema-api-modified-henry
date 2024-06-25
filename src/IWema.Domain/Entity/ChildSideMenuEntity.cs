using System.ComponentModel.DataAnnotations.Schema;

namespace IWema.Domain.Entity;

public class ChildSideMenuEntity : EntityBase
{
    public ChildSideMenuEntity() { } 

    public ChildSideMenuEntity(string name, string link, string icon,Guid parentSideMenuId)
    {
        Name = name;
        Link = link;
        Icon = icon;
        ParentSideMenuId = parentSideMenuId;
    }

    public string Name { get; set; }
    public string Link { get; set; }
    public string Icon { get; set; }
    public Guid ParentSideMenuId { get; set; }
   
}