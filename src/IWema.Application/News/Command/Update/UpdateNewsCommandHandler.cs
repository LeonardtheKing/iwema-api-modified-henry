using IWema.Application.Common.DTO;
using IWema.Application.Contract;
using IWema.Domain.Entity;
using MediatR;

namespace IWema.Application.News.Command.Update;

public record UpdateNewsCommand(Guid Id, string Title, string Content, bool IsActive) : IRequest<ServiceResponse>;

public class UpdateNewsCommandHandler(INewsRepository newsRepository) : IRequestHandler<UpdateNewsCommand, ServiceResponse>
{
    public async Task<ServiceResponse> Handle(UpdateNewsCommand command, CancellationToken cancellationToken)
    {
        NewsEntity news = await newsRepository.GetById(command.Id);

        if (news == null)
        {
            return new("News not found.", false);
        }

        news.Update(command.Title, command.Content, command.IsActive);

        var updated = await newsRepository.Update(news);

        return new(updated ? "Updated" : "Failed to update news.", updated);
    }
}