namespace IWema.Application.SideMenu.ParentSideMenu.Query.GetParentsWithChildren;

public class GetParentsQueryModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public IList<GetChildrenQueryModel> ChildSideMenu { get; set; }
}
