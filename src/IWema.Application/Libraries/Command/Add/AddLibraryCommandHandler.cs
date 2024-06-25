using IWema.Application.Common.DTO;
using IWema.Application.Common.Utilities;
using IWema.Application.Contract;
using IWema.Domain.Entity;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace IWema.Application.Libraries.Command.Add
{
    public record AddLibraryCommand(IFormFile File, string Description, string Type) : IRequest<ServiceResponse>;

    public class AddLibraryCommandHandler(ILibraryRepository libraryRepository,IFileHandler fileHandler) : IRequestHandler<AddLibraryCommand, ServiceResponse>
    {
        public async Task<ServiceResponse> Handle(AddLibraryCommand command, CancellationToken cancellationToken)
        {
            var saveFileResponse = await fileHandler.SaveFile(command.File);

            if (!saveFileResponse.Successful)
                return new("The File Upload was Unsuccessful");

            var library = Library.Create(saveFileResponse.Response, command.Description, command.Type);

            var added = await libraryRepository.Add(library);

            return new(added ? "Created" : "File upload failed.", added);
        }
    }
}
