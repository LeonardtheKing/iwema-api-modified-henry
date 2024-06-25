namespace IWema.Application.SideMenu.ChildSideMenu.Command.Add;

public class AddChildSideMenuInputModel
{
    public string Name { get; set; } = string.Empty;
    public string Link { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public Guid ParentSideMenuId { get; set; }
}
