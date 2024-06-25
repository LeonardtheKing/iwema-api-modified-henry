using IWema.Application.Common.DTO;
using IWema.Application.Contract;
using MediatR;

namespace IWema.Application.SideMenu.ChildSideMenu.Query.GetAllChildSideMenu;

public record GetAllChildSideMenuQuery : IRequest<ServiceResponse<List<GetAllChildSideMenuOutputModel>>>;
public class GetAllChildSideMenuQueryHandler : IRequestHandler<GetAllChildSideMenuQuery, ServiceResponse<List<GetAllChildSideMenuOutputModel>>>
{
    private readonly ISideMenuRepository _sideMenuRepository;
    public GetAllChildSideMenuQueryHandler(ISideMenuRepository sideMenuRepository)
    {

        _sideMenuRepository = sideMenuRepository;

    }
    public async Task<ServiceResponse<List<GetAllChildSideMenuOutputModel>>> Handle(GetAllChildSideMenuQuery request, CancellationToken cancellationToken)
    {
        var childSideMenu = await _sideMenuRepository.GetAllChildSideMenu();

        List<GetAllChildSideMenuOutputModel> result = childSideMenu.Select(x => new GetAllChildSideMenuOutputModel(x.Id, x.Name, x.Link, x.Icon)).ToList();

        return new("", true, result);
    }
}

