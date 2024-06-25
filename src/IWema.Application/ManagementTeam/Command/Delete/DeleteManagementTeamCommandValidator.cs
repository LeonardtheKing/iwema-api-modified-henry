using FluentValidation;
using IWema.Application.ManagementTeam.Command.Delete;

public class DeleteManagementTeamCommandValidator : AbstractValidator<DeleteManagementTeamCommand>
{
    public DeleteManagementTeamCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty().WithMessage("Id is required.")
            .Must(BeAValidGuid).WithMessage("Invalid Id format.")
             .Must(BeAGuid)
            .WithMessage("Invalid GUID format")
            .Must(id => id.ToString().Length == 36).WithMessage("Invalid GUID length. It should be 36 characters.");

    }

    private bool BeAValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
    private bool BeAGuid(Guid guid)
    {
        return Guid.TryParse(guid.ToString(), out _);
    }
}
