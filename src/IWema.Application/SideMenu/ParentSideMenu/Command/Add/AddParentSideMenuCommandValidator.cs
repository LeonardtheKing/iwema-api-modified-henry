using FluentValidation;
using IWema.Application.SideMenu.ParentSideMenu.Command.Add;

public class AddParentToggleInputModelValidator : AbstractValidator<AddParentSideMenuCommand>
{
    public AddParentToggleInputModelValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("SubMenu Name is required.")
            .MaximumLength(100).WithMessage("SubMenu Name cannot exceed 30 characters.");

        RuleFor(x => x.Icon)
           .NotEmpty().WithMessage("SubMenu Name is required.")
           .MaximumLength(100).WithMessage("SubMenu Name cannot exceed 30 characters.");
    }
}
