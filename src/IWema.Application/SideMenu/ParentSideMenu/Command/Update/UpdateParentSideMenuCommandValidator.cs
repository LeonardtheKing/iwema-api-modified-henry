using FluentValidation;
using IWema.Application.SideMenu.ParentSideMenu.Command.Update;

public class UpdateParentSideMenuCommandValidator : AbstractValidator<UpdateParentSideMenuCommand>
{
    public UpdateParentSideMenuCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty().WithMessage("Main menu ID must not be empty.");

        RuleFor(command => command.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

    }
}
