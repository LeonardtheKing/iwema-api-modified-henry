using IWema.Application.Common.DTO;
using IWema.Application.Contract;
using MediatR;

namespace IWema.Application.SideMenu.ParentSideMenu.Query.GetAllParentSideMenu;

public record GetAllParentSideMenuQuery : IRequest<ServiceResponse<List<GetAllParentsSideMenuOutputModel>>>;
public class GetAllParentSideMenuQueryHandler : IRequestHandler<GetAllParentSideMenuQuery, ServiceResponse<List<GetAllParentsSideMenuOutputModel>>>
{
    private readonly ISideMenuRepository _sideMenuRepository;
    public GetAllParentSideMenuQueryHandler(ISideMenuRepository sideMenuRepository)
    {
        _sideMenuRepository = sideMenuRepository;
    }
    public async Task<ServiceResponse<List<GetAllParentsSideMenuOutputModel>>> Handle(GetAllParentSideMenuQuery request, CancellationToken cancellationToken)
    {
        var parentSideMenu = await _sideMenuRepository.GetAllParentSideMenu();

        List<GetAllParentsSideMenuOutputModel> result = parentSideMenu.Select(x => new GetAllParentsSideMenuOutputModel(x.Id, x.Name,  x.Icon)).ToList();

        return new("", true, result);
    }
}
