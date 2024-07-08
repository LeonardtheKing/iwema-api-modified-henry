
using IWema.Application.Common.DTO;
using IWema.Application.Common.Utilities;
using IWema.Application.Contract;
using IWema.Domain.Entity;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace IWema.Application.Banners.Command.Add;

public record AddBannerCommand(IFormFile File, string Title, bool IsActive) : IRequest<ServiceResponse>;

public class AddBannerCommandHandler(IBannerRepository bannerRepository, IWebHostEnvironment env) : IRequestHandler<AddBannerCommand, ServiceResponse>
{
    public async Task<ServiceResponse> Handle(AddBannerCommand command, CancellationToken cancellationToken)
    {
        var saveFileResponse = await FileHandler.SaveFileAsync(command.File,env,cancellationToken);

        if (saveFileResponse == null || string.IsNullOrEmpty(saveFileResponse))
            return new("Unable to save file");

        var banner = Banner.Create(saveFileResponse, command.Title, command.IsActive);

        var added = await bannerRepository.Add(banner);

        return new(added ? "Created" : "File upload failed.", added);
    }
}