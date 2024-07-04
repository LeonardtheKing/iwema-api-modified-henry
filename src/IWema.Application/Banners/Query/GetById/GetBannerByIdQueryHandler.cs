using IWema.Application.Common.DTO;
using IWema.Application.Common.Utilities;
using IWema.Application.Contract;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace IWema.Application.Banners.Query.GetById;

public record GetBannerByIdQuery(Guid Id) : IRequest<ServiceResponse<GetBannerByIdQueryOutputModel>>;

public class GetBannerByIdQueryHandler(IBannerRepository bannerRepository,
    IHttpContextAccessor httpContextAccessor) : IRequestHandler<GetBannerByIdQuery, ServiceResponse<GetBannerByIdQueryOutputModel>>
{
    public async Task<ServiceResponse<GetBannerByIdQueryOutputModel>> Handle(GetBannerByIdQuery request, CancellationToken cancellationToken)
    {
        var httpRequest = httpContextAccessor.HttpContext.Request;

        var banner = await bannerRepository.GetById(request.Id);

        if (banner == null)
            return new("Record not found!");

        var r = FileHandler.GetFullDirectoryLocation(httpRequest, banner.Name);
        if (!r.Successful)
            return new("File not found!");

        GetBannerByIdQueryOutputModel result = new(banner.Id, FileHandler.GetFullDirectoryLocation(httpRequest, banner.Name).Response, banner.Title, banner.IsActive, banner.CreatedAt.ToString("g"));

        return new("", true, result);
    }
}