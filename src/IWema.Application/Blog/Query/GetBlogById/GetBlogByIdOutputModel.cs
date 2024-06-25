namespace IWema.Application.Blog.Query.GetBlogById;

public record GetBlogByIdOutputModel(Guid Id, string ImageLocation, string Title, string Summary, string ReadMoreLink);