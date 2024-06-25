using IWema.Application.Common.DTO;
using IWema.Application.Contract;
using IWema.Domain.Entity;
using MediatR;

namespace IWema.Application.News.Command.Add;

public record AddNewsCommand(string Title, string Content, bool IsActive = true) : IRequest<ServiceResponse>;

public class AddNewsCommandHandler(INewsRepository newsRepository) : IRequestHandler<AddNewsCommand, ServiceResponse>
{
    public async Task<ServiceResponse> Handle(AddNewsCommand command, CancellationToken cancellationToken)
    {
        NewsEntity news = new(command.Title, command.Content, command.IsActive);

        var added = await newsRepository.Add(news);

        return new(added ? "Created" : "Failed to add news.", added);
    }
}
