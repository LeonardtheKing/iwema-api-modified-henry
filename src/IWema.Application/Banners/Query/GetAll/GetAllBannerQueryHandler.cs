using IWema.Application.Common.DTO;
using IWema.Application.Common.Utilities;
using IWema.Application.Contract;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace IWema.Application.Banners.Query.GetAll;

public record GetAllBannerQuery : IRequest<ServiceResponse<List<GetAllBannerQueryOutputModel>>>;

public class GetAllBannerQueryHandler(IBannerRepository bannerRepository,
    IHttpContextAccessor httpContextAccessor) : IRequestHandler<GetAllBannerQuery, ServiceResponse<List<GetAllBannerQueryOutputModel>>>
{
    public async Task<ServiceResponse<List<GetAllBannerQueryOutputModel>>> Handle(GetAllBannerQuery request, CancellationToken cancellationToken)
    {
        var banners = await bannerRepository.Get();

        var httpRequest = httpContextAccessor.HttpContext.Request;

        List<GetAllBannerQueryOutputModel> result = banners.Select(x => new GetAllBannerQueryOutputModel(x.Id, FileHandler.GetFullDirectoryLocation(httpRequest, x.Name).Response, x.Title, x.IsActive, x.CreatedAt.ToString("g"))).ToList();

        return new("", true, result);
    }
}