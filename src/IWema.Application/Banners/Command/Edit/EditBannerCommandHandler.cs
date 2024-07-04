using IWema.Application.Common.DTO;
using IWema.Application.Common.Utilities;
using IWema.Application.Contract;
using IWema.Domain.Entity;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace IWema.Application.Banners.Command.Edit
{
    public record EditBannerCommand(Guid Id, IFormFile File, string Title, bool IsActive) : IRequest<ServiceResponse>;

    public class EditBannerCommandHandler(IBannerRepository bannerRepository) : IRequestHandler<EditBannerCommand, ServiceResponse>
    {
        public async Task<ServiceResponse> Handle(EditBannerCommand command, CancellationToken cancellationToken)
        {
            Banner banner = await bannerRepository.GetById(command.Id);

            if (banner == null)
            {
                return new("Banner record not found.", false);
            }

            var deleteResponse = await FileHandler.DeleteFileAsync(banner.Name);
            if (!deleteResponse.Successful)
            {
                return deleteResponse;
            }

            var saveFileResponse = await FileHandler.SaveFileAsync(command.File,cancellationToken);
            if (!saveFileResponse.Successful)
                return saveFileResponse;

            banner.Update(saveFileResponse.Response, command.Title, command.IsActive);

            var updated = await bannerRepository.Update(banner);

            return new(updated ? "Updated" : "File upload failed.", updated);
        }
    }
}