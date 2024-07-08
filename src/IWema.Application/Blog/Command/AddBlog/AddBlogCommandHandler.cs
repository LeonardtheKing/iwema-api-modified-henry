using IWema.Application.Common.DTO;
using IWema.Application.Common.Utilities;
using IWema.Application.Contract;
using IWema.Domain.Entity;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace IWema.Application.Blog.Command.AddBlog;

public record AddBlogCommand(IFormFile File, string Title, string Summary, string ReadMoreLink) : IRequest<ServiceResponse>;
public class AddBlogCommandHandler(IBlogRepository blogRepository,IHttpContextAccessor httpContextAccessor, IWebHostEnvironment env) : IRequestHandler<AddBlogCommand, ServiceResponse>
{
    public async Task<ServiceResponse> Handle(AddBlogCommand command, CancellationToken cancellationToken)
    {
        var response = await FileHandler.SaveFileAsync(command.File,env,cancellationToken);
        if (response == null)
            return new("Image upload failed");
        var imageUrl = await FileHandler.GetImageUrlAsync(command.File,httpContextAccessor,env );
        if (imageUrl == null || string.IsNullOrEmpty(imageUrl))
            return new("Image upload failed");

        BlogEntity blog = new(imageUrl, command.Title, command.Summary, command.ReadMoreLink);

        var added = await blogRepository.Add(blog);

        return new(added ? "Created" : "Blog upload failed.", added);
    }

    //public Task<ServiceResponse> Handle(AddBlogCommand request, CancellationToken cancellationToken)
    //{
    //    throw new NotImplementedException();
    //}
}
