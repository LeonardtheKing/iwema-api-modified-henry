using IWema.Application.Common.DTO;
using IWema.Application.Common.Utilities;
using IWema.Application.Contract;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace IWema.Application.Blog.Command.UpdateBlog;


public class UpdateBlogCommandHandler(IBlogRepository blogRepository,IHttpContextAccessor httpContextAccessor) : IRequestHandler<UpdateBlogCommand, ServiceResponse>
{
    public async Task<ServiceResponse> Handle(UpdateBlogCommand command, CancellationToken cancellationToken)
    {
        var blog = await blogRepository.GetById(command.Id);
        if (blog == null)
            return new("blog not found.", false);

        if (command.File != null)
        {
            var updatedImageLocation = await FileHandler.UpdateImageAsync(command.File,httpContextAccessor);
            blog.UpdateImageLocation(updatedImageLocation);
        }

        if (command.Title != null)
            blog.UpdateTitle(command.Title);

        if (command.Summary != null)
            blog.UpdateSummary(command.Summary);

        if (command.ReadMoreLink != null)
            blog.UpdateReadMoreLink(command.ReadMoreLink);

        var updated = await blogRepository.UpdateAsync(command.Id, blog);
        if (updated == 0)
            return new("Blog  was not successfully updated", false);

        return new("Blog was Updated Successfully", true);
    }
}
