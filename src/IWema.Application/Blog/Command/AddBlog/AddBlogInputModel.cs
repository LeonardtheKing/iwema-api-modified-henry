using Microsoft.AspNetCore.Http;

namespace IWema.Application.Blog.Command.AddBlog;

public class AddBlogInputModel
{
    public IFormFile File { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string ReadMoreLink { get; set; } = string.Empty;
}


