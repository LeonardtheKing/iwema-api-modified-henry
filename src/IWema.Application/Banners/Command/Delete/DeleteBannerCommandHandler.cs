using IWema.Application.Common.DTO;
using IWema.Application.Contract;
using MediatR;

namespace IWema.Application.Banners.Command.Delete;

public record DeleteBannerCommand(Guid Id) : IRequest<ServiceResponse>;

public class DeleteBannerCommandHandler(IBannerRepository bannerRepository, IFileHandler fileHandler) : IRequestHandler<DeleteBannerCommand, ServiceResponse>
{
    public async Task<ServiceResponse> Handle(DeleteBannerCommand command, CancellationToken cancellationToken)
    {

        var banner = await bannerRepository.GetById(command.Id);
        if (banner == null)
            return new("Banner  not found.");

        var delete = await bannerRepository.Delete(command.Id);
        if (!delete)
            return new("An error occurred while deleting the Banner.", false);
        return new("Banner deleted successfully.", true);
    }
}