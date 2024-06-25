using FluentValidation;

namespace IWema.Application.News.Command.Add;

public class AddNewsCommandValidator : AbstractValidator<AddNewsCommand>
{
    public AddNewsCommandValidator()
    {
        RuleFor(x => x.Title)
            .MinimumLength(3)
            .NotEmpty();

        RuleFor(x => x.Content)
            .MinimumLength(3)
            .NotEmpty();
    }
}