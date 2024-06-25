using IWema.Application.Common.DTO;
using IWema.Application.Contract;
using IWema.Domain.Entity;
using MediatR;

namespace IWema.Application.News.Command.Delete
{
    public record DeleteNewsCommand(Guid Id) : IRequest<ServiceResponse>;

    public class DeleteNewsCommandHandler(INewsRepository newsScrollRepository) : IRequestHandler<DeleteNewsCommand, ServiceResponse>
    {
        public async Task<ServiceResponse> Handle(DeleteNewsCommand command, CancellationToken cancellationToken)
        {
            NewsEntity news = await newsScrollRepository.GetById(command.Id);

            if (news == null)
            {
                return new("News not found.", false);
            }

            var deleted = await newsScrollRepository.Delete(command.Id);

            return new(deleted ? "Deleted" : "Failed to delete news.", deleted);
        }
    }
}
