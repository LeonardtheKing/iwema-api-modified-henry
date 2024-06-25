using IWema.Application.Common.DTO;
using IWema.Application.Contract;
using IWema.Domain.Entity;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace IWema.Application.Blog.Command.AddBlog;

//internal class AddBlogCommandHandler
//{
//}

public record AddBlogCommand(IFormFile File, string Title, string Summary, string ReadMoreLink) : IRequest<ServiceResponse>;
public class AddBlogCommandHandler(IBlogRepository blogRepository, IFileHandler fileHandler) : IRequestHandler<AddBlogCommand, ServiceResponse>
{
    public async Task<ServiceResponse> Handle(AddBlogCommand command, CancellationToken cancellationToken)
    {
        var response = await fileHandler.SaveFile(command.File);
        if (response == null)
            return new("Image upload failed");
        var imageUrl = await fileHandler.GetImageUrl(command.File);
        if (imageUrl == null || string.IsNullOrEmpty(imageUrl))
            return new("Image upload failed");

        BlogEntity blog = new(imageUrl, command.Title, command.Summary, command.ReadMoreLink);

        var added = await blogRepository.Add(blog);

        return new(added ? "Created" : "Blog upload failed.", added);
    }
}
