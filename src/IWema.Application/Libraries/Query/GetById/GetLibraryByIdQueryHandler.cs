using IWema.Application.Common.DTO;
using IWema.Application.Contract;
using IWema.Domain.Entity;
using MediatR;


namespace IWema.Application.Libraries.Query.GetById
{
    public record GetLibraryByIdQuery(Guid Id) : IRequest<ServiceResponse<GetLibraryByIdQueryOutputModel>>;

    public class GetLibraryByIdQueryHandler(ILibraryRepository libraryRepository) : IRequestHandler<GetLibraryByIdQuery, ServiceResponse<GetLibraryByIdQueryOutputModel>>
    {
        public async Task<ServiceResponse<GetLibraryByIdQueryOutputModel>> Handle(GetLibraryByIdQuery request, CancellationToken cancellationToken)
        {
            Library library = await libraryRepository.GetById(request.Id);

            if (library == null)
                return new("Record not found!");

            GetLibraryByIdQueryOutputModel result = new(library.Id, library.Description, library.Type, library.CreatedAt.ToString("g"));

            return new("", true, result);
        }
    }
}
