using IWema.Application.Common.DTO;
using IWema.Application.Contract;
using MediatR;

namespace IWema.Application.News.Query.GetById;

public record GetNewsByIdQuery(Guid Id) : IRequest<ServiceResponse<GetNewsByIdQueryOutputModel>>;

public class GetNewsByIdQueryHandler(INewsRepository newsRepository) : IRequestHandler<GetNewsByIdQuery, ServiceResponse<GetNewsByIdQueryOutputModel>>
{
    public async Task<ServiceResponse<GetNewsByIdQueryOutputModel>> Handle(GetNewsByIdQuery request, CancellationToken cancellationToken)
    {
        var news = await newsRepository.GetById(request.Id);

        if (news == null)
            return new("News not found.");

        GetNewsByIdQueryOutputModel response = new(news.Id, news.Title, news.Content, news.IsActive, news.CreatedAt);

        return new("", true, response);
    }

}
