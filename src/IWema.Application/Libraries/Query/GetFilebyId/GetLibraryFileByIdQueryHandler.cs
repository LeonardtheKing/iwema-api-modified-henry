using IWema.Application.Common.DTO;
using IWema.Application.Common.Utilities;
using IWema.Application.Contract;
using IWema.Application.Libraries.Query.GetFileById;
using MediatR;
using MimeMapping;

namespace IWema.Application.Libraries.Query.GetFilebyId
{
    public record GetLibraryFileByIdQuery(Guid Id) : IRequest<ServiceResponse<GetLibraryFileByIdQueryOutputModel>>;

    public class GetLibraryFileByIdQueryHandler(ILibraryRepository libraryRepository) : IRequestHandler<GetLibraryFileByIdQuery, ServiceResponse<GetLibraryFileByIdQueryOutputModel>>
    {
        public async Task<ServiceResponse<GetLibraryFileByIdQueryOutputModel>> Handle(GetLibraryFileByIdQuery request, CancellationToken cancellationToken)
        {
            var library = await libraryRepository.GetById(request.Id);

            if (library == null)
                return new("File not found.");

            var outputStream = new MemoryStream();

            var fileExistResponse = await FileHandler.ReadFileAsync(library.Name, outputStream);

            if (!fileExistResponse.Successful)
                return new("File not found.");

            var extension = MimeUtility.GetMimeMapping(library.Name);
            outputStream.Seek(0, SeekOrigin.Begin);

            GetLibraryFileByIdQueryOutputModel response = new(outputStream, extension, library.Name);

            return new("", true, response);
        }
    }
}
