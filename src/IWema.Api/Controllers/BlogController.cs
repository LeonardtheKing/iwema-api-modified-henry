using IWema.Application.Blog.Command.AddBlog;
using IWema.Application.Blog.Command.DeleteBlog;
using IWema.Application.Blog.Command.UpdateBlog;
using IWema.Application.Blog.Query.GetAllBlogs;
using IWema.Application.Blog.Query.GetBlogById;
using IWema.Domain.Entity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IWema.Api.Controllers;

[Route("api/[controller]")]
[Authorize]
public class BlogController(IMediator mediator) : BaseController
{

    [HttpPost]
    public async Task<IActionResult> Add([FromForm] AddBlogInputModel request)
    {
        var command = new AddBlogCommand(request.File, request.Title, request.Summary, request.ReadMoreLink);
        var response = await mediator.Send(command);
        return ServiceResponse(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var response = await mediator.Send(new DeleteBlogCommand(id));
        return ServiceResponse(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var response = await mediator.Send(new GetBlogByIdquery(id));
        return ServiceResponse(response);
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var response = await mediator.Send(new GetAllBlogsQuery());
        return ServiceResponse(response);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromForm] UpdateBlogInputModel request)
    {
        UpdateBlogCommand command = new(request.Id, request.File, request.Title, request.Summary, request.ReadMoreLink);
        var response = await mediator.Send(command);

        return ServiceResponse(response);
    }


}


