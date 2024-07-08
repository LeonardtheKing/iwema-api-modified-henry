using IWema.Application.Common.DTO;
using IWema.Application.Common.Utilities;
using IWema.Application.Contract;
using IWema.Domain.Entity;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace IWema.Application.Libraries.Command.Add
{
    public record AddLibraryCommand(IFormFile File, string Description, string Type) : IRequest<ServiceResponse>;

    public class AddLibraryCommandHandler(ILibraryRepository libraryRepository, IWebHostEnvironment env) : IRequestHandler<AddLibraryCommand, ServiceResponse>
    {
        public async Task<ServiceResponse> Handle(AddLibraryCommand command, CancellationToken cancellationToken)
        {
            var saveFileResponse = await FileHandler.SaveFileAsync(command.File,env,cancellationToken);

            if (saveFileResponse==null || string.IsNullOrEmpty(saveFileResponse))
                return new("The File Upload was Unsuccessful");

            var library = Library.Create(saveFileResponse, command.Description, command.Type);

            var added = await libraryRepository.Add(library);

            return new(added ? "Created" : "File upload failed.", added);
        }
    }
}
