using IWema.Application.Common.DTO;
using IWema.Application.Contract;
using MediatR;

namespace IWema.Application.SideMenu.ParentSideMenu.Query.GetParentSideMenu;

public record GetParentSideMenuByIdQuery(Guid Id) : IRequest<ServiceResponse<GetParentSideMenuOutputQueryModel>>;

public class GetParentSideMenuQueryHandler(ISideMenuRepository sideMenuRepository) : IRequestHandler<GetParentSideMenuByIdQuery, ServiceResponse<GetParentSideMenuOutputQueryModel>>
{
    public async Task<ServiceResponse<GetParentSideMenuOutputQueryModel>> Handle(GetParentSideMenuByIdQuery query, CancellationToken cancellationToken)
    {
        var result = await sideMenuRepository.GetParentSideMenu(query.Id);

        if (result.parentSideMenu == null)
        {
            return new ServiceResponse<GetParentSideMenuOutputQueryModel>("Parent Side Menu not found", false);
        }

        if (result.childSideMenu == null)
        {
            return new ServiceResponse<GetParentSideMenuOutputQueryModel>("Child Side Menu not found", false);
        }

        var subMenuModels = new List<GetChildideMenuByIdQueryModel>();

        foreach (var subMenuEntity in result.childSideMenu)
        {
            var subMenuModel = new GetChildideMenuByIdQueryModel
            {
                Id = subMenuEntity.Id,
                Name = subMenuEntity.Name,
                Link = subMenuEntity.Link,
                Icon = subMenuEntity.Icon
            };
            subMenuModels.Add(subMenuModel);
        }

        var model = new GetParentSideMenuOutputQueryModel
        {
            Id = result.parentSideMenu.Id,
            Name = result.parentSideMenu.Name,
            Icon = result.parentSideMenu.Icon,
            ChildSideMenu = subMenuModels
        };

        return new ServiceResponse<GetParentSideMenuOutputQueryModel>("Parent Side Menu retrieved successfully", true, model);
    }
}
