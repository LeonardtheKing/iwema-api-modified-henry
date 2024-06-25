using IWema.Application.Common.DTO;
using IWema.Application.Contract;
using MediatR;

namespace IWema.Application.Blog.Query.GetBlogById;



public record GetBlogByIdquery(Guid Id) : IRequest<ServiceResponse<GetBlogByIdOutputModel>>;
public class GetBlogByIdQueryHandler(IBlogRepository blogRepository) : IRequestHandler<GetBlogByIdquery, ServiceResponse<GetBlogByIdOutputModel>>
{
    public async Task<ServiceResponse<GetBlogByIdOutputModel>> Handle(GetBlogByIdquery query, CancellationToken cancellationToken)
    {
        var blog = await blogRepository.GetById(query.Id);
        if (blog == null)
            return new("Record not found!");
        GetBlogByIdOutputModel response = new(blog.Id, blog.ImageLocation, blog.Title, blog.Summary, blog.ReadMoreLink);

        return new("", true, response);
    }
}