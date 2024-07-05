using IWema.Application.News.Command.Add;
using IWema.Application.News.Command.Delete;
using IWema.Application.News.Command.Update;
using IWema.Application.News.Query.GetAll;
using IWema.Application.News.Query.GetById;
using IWema.Application.News.Query.GetScroll;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IWema.Api.Controllers;

public class NewsController(IMediator mediator) : BaseController
{
    [HttpGet("scroll")]
    public async Task<IActionResult> GetNewsScroll()
    {
        var response = await mediator.Send(new GetNewsScrollQuery());
        return ServiceResponse(response);
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var response = await mediator.Send(new GetAllNewsQuery());
        return ServiceResponse(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var response = await mediator.Send(new GetNewsByIdQuery(id));
        return ServiceResponse(response);
    }

    [HttpPost]
    public async Task<IActionResult> Add(AddNewsInputModel request)
    {
        AddNewsCommand command = new(request.Title, request.Content, request.IsActive);
        var response = await mediator.Send(command);
        return ServiceResponse(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        DeleteNewsCommand command = new(id);
        var response = await mediator.Send(command);
        return ServiceResponse(response);
    }

    [HttpPut]
    public async Task<IActionResult> Put(UpdateNewsInputModel request)
    {
        UpdateNewsCommand command = new(request.Id, request.Title, request.Content, request.IsActive);
        var response = await mediator.Send(command);
        return ServiceResponse(response);
    }
}
