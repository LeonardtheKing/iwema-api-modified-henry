using IWema.Application.Common.DTO;
using IWema.Application.Contract;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace IWema.Application.Libraries.Query.GetAll;

public record GetAllLibraryQuery : IRequest<ServiceResponse<GetAllLibraryQueryOutputModel>>;

public class GetAllLibraryQueryHandler(ILibraryRepository libraryRepository, IHttpContextAccessor httpContextAccessor,
    IFileHandler fileHandler) : IRequestHandler<GetAllLibraryQuery, ServiceResponse<GetAllLibraryQueryOutputModel>>
{
    public async Task<ServiceResponse<GetAllLibraryQueryOutputModel>> Handle(GetAllLibraryQuery request, CancellationToken cancellationToken)
    {
        var httpRequest = httpContextAccessor.HttpContext!.Request;


        var libraries = await libraryRepository.Get();

        List<GetLibraryModel> resultList = [.. libraries.Select(x => new GetLibraryModel(x.Id, fileHandler.GetFullDirectoryLocation(httpRequest, x.Name).Response, x.Description, x.Type, x.CreatedAt.ToString("g"))).OrderBy(o => o.Type)];

        GetAllLibraryQueryOutputModel sortedResult = new(resultList.Where(w => w.Type == LibraryTypeEnum.Form.ToString()).OrderBy(a => a.Description),
            resultList.Where(w => w.Type == LibraryTypeEnum.PolicyManual.ToString()).OrderBy(a => a.Description), resultList.Where(w => w.Type == LibraryTypeEnum.Template.ToString()).OrderBy(a => a.Description),
            resultList.Where(w => w.Type == LibraryTypeEnum.Report.ToString()).OrderBy(a => a.Description), resultList.Where(w => w.Type == LibraryTypeEnum.Letter.ToString()).OrderBy(a => a.Description),
            resultList.Where(w => w.Type == LibraryTypeEnum.ProductCompendium.ToString()).OrderBy(a => a.Description), resultList.Where(w => w.Type == LibraryTypeEnum.Appendix.ToString()).OrderBy(a => a.Description));

        return new("", true, sortedResult);
    }
}

