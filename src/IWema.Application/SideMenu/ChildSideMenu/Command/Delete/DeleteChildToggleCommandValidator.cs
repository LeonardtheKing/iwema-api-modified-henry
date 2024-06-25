using FluentValidation;
using IWema.Application.SideMenu.ChildSideMenu.Command.Delete;

public class DeleteChildToggleCommandValidator : AbstractValidator<DeleteChildToggleCommand>
{
    public DeleteChildToggleCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty().WithMessage("Id is required.")
            .NotEqual(Guid.Empty).WithMessage("Id cannot be empty.");
    }
}
