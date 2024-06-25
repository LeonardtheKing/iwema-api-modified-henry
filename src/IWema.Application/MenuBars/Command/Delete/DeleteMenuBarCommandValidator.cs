using FluentValidation;

namespace IWema.Application.MenuBars.Command.Delete;

public class DeleteMenuBarCommandValidator : AbstractValidator<DeleteMenuBarCommand>
{
    public DeleteMenuBarCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}