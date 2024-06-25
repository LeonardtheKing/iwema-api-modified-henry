using IWema.Application.Common.DTO;
using IWema.Application.Contract;
using MediatR;

namespace IWema.Application.Blog.Command.DeleteBlog;


public record DeleteBlogCommand(Guid Id) : IRequest<ServiceResponse>;
public class DeleteBlogCommandHandler(IBlogRepository blogRepository) : IRequestHandler<DeleteBlogCommand, ServiceResponse>
{
    public async Task<ServiceResponse> Handle(DeleteBlogCommand request, CancellationToken cancellationToken)
    {
        var blog = await blogRepository.GetById(request.Id);
        if (blog == null)
            return new("Blog  not found.");

        var delete = await blogRepository.Delete(request.Id);
        if (!delete)
            return new("An error occurred while deleting the Blog .");
        return new("Blog  deleted successfully.", true);
    }
}
