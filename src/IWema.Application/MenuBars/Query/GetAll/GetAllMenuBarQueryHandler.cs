using IWema.Application.Common.DTO;
using IWema.Application.Contract;
using MediatR;

namespace IWema.Application.MenuBars.Query.GetAll;

public record GetAllMenuBarQuery : IRequest<ServiceResponse<List<GetAllMenuBarOutputModel>>>;

public class GetAllMenuBarQueryHandler(IMenuBarRepository menuBarRepository) : IRequestHandler<GetAllMenuBarQuery, ServiceResponse<List<GetAllMenuBarOutputModel>>>
{
    public async Task<ServiceResponse<List<GetAllMenuBarOutputModel>>> Handle(GetAllMenuBarQuery request, CancellationToken cancellationToken)
    {
        var menuBars = await menuBarRepository.Get();

        List<GetAllMenuBarOutputModel> result = menuBars.Select(x => new GetAllMenuBarOutputModel(x.Id, x.Name, x.Link, x.Icon,x.Description)).ToList();

        return new("", true, result);
    }
}