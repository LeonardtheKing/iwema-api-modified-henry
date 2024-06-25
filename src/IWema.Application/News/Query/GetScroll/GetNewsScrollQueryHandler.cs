using IWema.Application.Common.DTO;
using IWema.Application.Contract;
using MediatR;

namespace IWema.Application.News.Query.GetScroll;

public record GetNewsScrollQuery : IRequest<ServiceResponse<GetNewsScrollQueryOutputModel>>;

public class GetNewsScrollQueryHandler(INewsRepository newsRepository) : IRequestHandler<GetNewsScrollQuery, ServiceResponse<GetNewsScrollQueryOutputModel>>
{
    public async Task<ServiceResponse<GetNewsScrollQueryOutputModel>> Handle(GetNewsScrollQuery request, CancellationToken cancellationToken)
    {
        var news = await newsRepository.GetActive();

        var scroll = news
            .Select(x => x.Content).ToList();

        GetNewsScrollQueryOutputModel result = new(string.Join(" | ", scroll));

        return new("", true, result);
    }
}