using FluentValidation;

namespace IWema.Application.News.Command.Update;

public class UpdateNewsScrollCommandValidator : AbstractValidator<UpdateNewsCommand>
{
    public UpdateNewsScrollCommandValidator()
    {

        RuleFor(x => x.Title)
            .MinimumLength(3)
            .NotEmpty();

        RuleFor(x => x.Content)
            .MinimumLength(3)
            .NotEmpty();
    }
}