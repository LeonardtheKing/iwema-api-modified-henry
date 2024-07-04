
using IWema.Application.Common.DTO;
using IWema.Application.Common.Utilities;
using IWema.Application.Contract;
using IWema.Domain.Entity;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace IWema.Application.Banners.Command.Add;

public record AddBannerCommand(IFormFile File, string Title, bool IsActive) : IRequest<ServiceResponse>;

public class AddBannerCommandHandler(IBannerRepository bannerRepository) : IRequestHandler<AddBannerCommand, ServiceResponse>
{
    public async Task<ServiceResponse> Handle(AddBannerCommand command, CancellationToken cancellationToken)
    {
        var saveFileResponse = await FileHandler.SaveFileAsync(command.File,cancellationToken);

        if (!saveFileResponse.Successful)
            return saveFileResponse;

        var banner = Banner.Create(saveFileResponse.Response, command.Title, command.IsActive);

        var added = await bannerRepository.Add(banner);

        return new(added ? "Created" : "File upload failed.", added);
    }
}