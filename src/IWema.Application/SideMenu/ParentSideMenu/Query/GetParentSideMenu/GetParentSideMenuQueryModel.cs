namespace IWema.Application.SideMenu.ParentSideMenu.Query.GetParentSideMenu;

public class GetParentSideMenuOutputQueryModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public IList<GetChildideMenuByIdQueryModel> ChildSideMenu { get; set; }

}