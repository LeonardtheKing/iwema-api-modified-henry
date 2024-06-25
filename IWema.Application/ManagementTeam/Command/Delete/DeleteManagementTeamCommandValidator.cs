using FluentValidation;
using IWema.Application.ManagementTeam.Command.Delete;

public class DeleteManagementTeamCommandValidator : AbstractValidator<DeleteManagementTeamCommand>
{
    public DeleteManagementTeamCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty().WithMessage("Id is required.")
            .Must(BeAValidGuid).WithMessage("Invalid Id format.");
    }

    private bool BeAValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}
