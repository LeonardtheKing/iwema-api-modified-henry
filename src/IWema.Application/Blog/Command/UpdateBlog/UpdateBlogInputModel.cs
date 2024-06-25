using Microsoft.AspNetCore.Http;

namespace IWema.Application.Blog.Command.UpdateBlog;

public record UpdateBlogInputModel
(
     Guid Id,
    IFormFile? File=null,
     string? Title=null,
     string? Summary = null,
     string? ReadMoreLink = null
    );
