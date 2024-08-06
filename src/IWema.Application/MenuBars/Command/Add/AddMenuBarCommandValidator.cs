using FluentValidation;

namespace IWema.Application.MenuBars.Command.Add;

public class AddMenuBarCommandValidator : AbstractValidator<AddMenuBarCommand>
{
    public AddMenuBarCommandValidator()
    {
        RuleFor(x => x.Name)
            .MinimumLength(1)
            .NotEmpty();

        RuleFor(x => x.Link)
           .NotEmpty()
           .WithMessage("Link is required");
    }

 
}