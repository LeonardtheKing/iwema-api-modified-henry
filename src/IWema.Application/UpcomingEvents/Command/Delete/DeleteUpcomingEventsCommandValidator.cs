using FluentValidation;

namespace IWema.Application.UpcomingEvents.Command.Delete;

public class DeleteUpcomingEventsCommandValidator : AbstractValidator<DeleteUpcomingEventsCommand>
{

    public DeleteUpcomingEventsCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();


    }


}

