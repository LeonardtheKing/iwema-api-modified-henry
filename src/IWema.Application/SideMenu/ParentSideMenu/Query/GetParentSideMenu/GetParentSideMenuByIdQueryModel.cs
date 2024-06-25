namespace IWema.Application.SideMenu.ParentSideMenu.Query.GetParentSideMenu;

public class GetChildideMenuByIdQueryModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Link { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;

}