using AutoMapper;
using IWema.Application.Common.Configuration;
using IWema.Application.Common.DTO;
using IWema.Application.Contract;
using MediatR;
using Microsoft.Extensions.Options;

namespace IWema.Application.MenuBars.Query.GetFavourite;

public record GetFavoriteQuery : IRequest<ServiceResponse<List<GetFavoriteOutputModel>>>;

public class GetFavoriteQueryHandler(IMenuBarRepository menuBarRepository, IMapper mapper, IOptions<GeneralConfigOptions> options) : IRequestHandler<GetFavoriteQuery, ServiceResponse<List<GetFavoriteOutputModel>>>
{
    public async Task<ServiceResponse<List<GetFavoriteOutputModel>>> Handle(GetFavoriteQuery request, CancellationToken cancellationToken)
    {
        var menuBars = await menuBarRepository.GetTop(options.Value.FavouriteHitCount);

        var model = mapper.Map<List<GetFavoriteOutputModel>>(menuBars);

        return new("successful", true, model);
    }
}