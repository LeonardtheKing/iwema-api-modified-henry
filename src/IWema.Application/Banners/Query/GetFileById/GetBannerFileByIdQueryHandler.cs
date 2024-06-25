using IWema.Application.Common.DTO;
using IWema.Application.Common.Utilities;
using IWema.Application.Contract;
using MediatR;
using MimeMapping;

namespace IWema.Application.Banners.Query.GetFileById;

public record GetBannerFileByIdQuery(Guid Id) : IRequest<ServiceResponse<GetBannerFileByIdQueryOutputModel>>;

public class GetBannerFileByIdQueryHandler(IBannerRepository bannerRepository, IFileHandler fileHandler) : IRequestHandler<GetBannerFileByIdQuery, ServiceResponse<GetBannerFileByIdQueryOutputModel>>
{
    public async Task<ServiceResponse<GetBannerFileByIdQueryOutputModel>> Handle(GetBannerFileByIdQuery request, CancellationToken cancellationToken)
    {
        var banner = await bannerRepository.GetById(request.Id);

        if (banner == null)
            return new("File not found.");

        var outputStream = new MemoryStream();

        var fileExistResponse = await fileHandler.ReadFile(banner.Name, outputStream);

        if (!fileExistResponse.Successful)
            return new("File not found.");

        var extension = MimeUtility.GetMimeMapping(banner.Name);
        outputStream.Seek(0, SeekOrigin.Begin);

        GetBannerFileByIdQueryOutputModel response = new(outputStream, extension, banner.Name);

        return new("", true, response);
    }
}