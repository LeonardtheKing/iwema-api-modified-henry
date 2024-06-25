namespace IWema.Application.SideMenu.ChildSideMenu.Command.Update;

public class UpdateChildSideMenuInputModel
{
    public Guid ParentToggleId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Link { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
}
