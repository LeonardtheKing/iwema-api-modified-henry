using IWema.Application.Common.DTO;
using IWema.Application.Contract;
using MediatR;

namespace IWema.Application.Blog.Query.GetAllBlogs;

public record GetAllBlogsQuery : IRequest<ServiceResponse<List<GetAllBlogsOutputModel>>>;
public class GetAllBlogsQueryHandler(IBlogRepository blogRepository) : IRequestHandler<GetAllBlogsQuery, ServiceResponse<List<GetAllBlogsOutputModel>>>
{
    public async Task<ServiceResponse<List<GetAllBlogsOutputModel>>> Handle(GetAllBlogsQuery query, CancellationToken cancellationToken)
    {
        var blogs = await blogRepository.Get();
        List<GetAllBlogsOutputModel> result = blogs.Select(x => new GetAllBlogsOutputModel(x.Id, x.ImageLocation, x.Title, x.Summary, x.ReadMoreLink)).ToList();

        return new("", true, result);
    }
}
