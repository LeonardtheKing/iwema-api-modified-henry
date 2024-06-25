using FluentValidation;

namespace IWema.Application.SideMenu.ParentSideMenu.Command.Delete;

public class DeleteParentToggleCommandValidator : AbstractValidator<DeleteParentToggleCommand>
{
    public DeleteParentToggleCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
