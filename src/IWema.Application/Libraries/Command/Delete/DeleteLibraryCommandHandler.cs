using IWema.Application.Common.DTO;
using IWema.Application.Common.Utilities;
using IWema.Application.Contract;
using IWema.Domain.Entity;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace IWema.Application.Libraries.Command.Delete;

public record DeleteLibraryCommand(Guid Id) : IRequest<ServiceResponse>;

public class DeleteLibraryCommandHandler(ILibraryRepository libraryRepository) : IRequestHandler<DeleteLibraryCommand, ServiceResponse>
{
    public async Task<ServiceResponse> Handle(DeleteLibraryCommand command, CancellationToken cancellationToken)
    {
        Library banner = await libraryRepository.GetById(command.Id);

        if (banner == null)
        {
            return new("Library record not found.", false);
        }

        var deleteResponse = await FileHandler.DeleteFileAsync(banner.Name);
        if (!deleteResponse.Successful)
        {
            return deleteResponse;
        }

        await libraryRepository.Delete(command.Id);

        return new("", true);
    }
}
