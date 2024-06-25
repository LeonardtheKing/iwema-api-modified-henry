using IWema.Application.Common.DTO;
using IWema.Application.Contract;
using MediatR;

namespace IWema.Application.SideMenu.ParentSideMenu.Query.GetParentsWithChildren;
public record GetParentsWithChildrenQuery: IRequest<ServiceResponse<List<GetParentsQueryModel>>>;
public class GetParentsWithChildrenQueryHandler(ISideMenuRepository sideMenuRepository) : IRequestHandler<GetParentsWithChildrenQuery, ServiceResponse<List<GetParentsQueryModel>>>
{
    public async Task<ServiceResponse<List<GetParentsQueryModel>>> Handle(GetParentsWithChildrenQuery request, CancellationToken cancellationToken)
    {
        var parentSideMenus = await sideMenuRepository.GetAllParentMenusWithChildren();

        if (parentSideMenus == null || !parentSideMenus.Any())
        {
            return new("No Parent Side Menus found");
           
        }

        var outputModels = new List<GetParentsQueryModel>();
        foreach (var parentMenu in parentSideMenus)
        {
            var childMenuModels = new List<GetChildrenQueryModel>();
            foreach (var child in parentMenu.childSideMenu)
            {
                childMenuModels.Add(new GetChildrenQueryModel
                {
                    Id = child.Id,
                    Name = child.Name,
                    Link = child.Link,
                    Icon = child.Icon
                });
            }

            var outputModel = new GetParentsQueryModel
            {
                Id = parentMenu.parentSideMenu.Id,
                Name = parentMenu.parentSideMenu.Name,
                Icon = parentMenu.parentSideMenu.Icon,
                ChildSideMenu = childMenuModels
                
            };
            outputModels.Add(outputModel);
        }

        return new ( "Parent Side Menus retrieved successfully",true, outputModels);
    }
}
