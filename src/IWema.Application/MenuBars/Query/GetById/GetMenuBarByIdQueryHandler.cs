using IWema.Application.Common.DTO;
using IWema.Application.Contract;
using MediatR;

namespace IWema.Application.MenuBars.Query.GetById;

public record GetMenuBarByIdQuery(Guid Id) : IRequest<ServiceResponse<GetMenuBarByIdQueryOutputModel>>;

public class GetMenuBarByIdQueryHandler(IMenuBarRepository menuBarRepository) : IRequestHandler<GetMenuBarByIdQuery, ServiceResponse<GetMenuBarByIdQueryOutputModel>>
{
    public async Task<ServiceResponse<GetMenuBarByIdQueryOutputModel>> Handle(GetMenuBarByIdQuery request, CancellationToken cancellationToken)
    {
        var menu = await menuBarRepository.GetById(request.Id);

        if (menu == null)
            return new("Record not found!");

        GetMenuBarByIdQueryOutputModel response = new(menu.Id, menu.Name, menu.Link);

        return new("", true, response);
    }
}
