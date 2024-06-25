using IWema.Application.Common.DTO;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace IWema.Application.Blog.Command.UpdateBlog;


public record UpdateBlogCommand(
       Guid Id,
        IFormFile? File = null,
       string? Title = null,
       string? Summary = null,
       string? ReadMoreLink = null      
   ) : IRequest<ServiceResponse>;
