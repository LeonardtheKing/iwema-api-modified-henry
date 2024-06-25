using IWema.Application.Common.DTO;
using IWema.Application.Contract;
using MediatR;

namespace IWema.Application.News.Query.GetAll;

public record GetAllNewsQuery() : IRequest<ServiceResponse<List<GetAllNewsQueryOutputModel>>>;

public class GetAllNewsQueryHandler(INewsRepository newScrollRepository) : IRequestHandler<GetAllNewsQuery, ServiceResponse<List<GetAllNewsQueryOutputModel>>>
{
    public async Task<ServiceResponse<List<GetAllNewsQueryOutputModel>>> Handle(GetAllNewsQuery request, CancellationToken cancellationToken)
    {
        var news = await newScrollRepository.Get();

        List<GetAllNewsQueryOutputModel> result = [
            .. news.Select(x => new GetAllNewsQueryOutputModel(x.Id, x.Content, x.Title, x.IsActive, x.CreatedAt))
            .OrderByDescending(o => o.CreatedAt)
        ];

        return new("", true, result);
    }
}