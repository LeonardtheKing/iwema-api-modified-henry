using FluentValidation;
using IWema.Application.ManagementTeam.Query.GetById;

public class GetManagementTeamByIdQueryValidator : AbstractValidator<GetManagementTeamByIdQuery>
{
    public GetManagementTeamByIdQueryValidator()
    {
        RuleFor(query => query.Id)
            .NotEmpty().WithMessage("Id cannot be empty.")
            .NotNull().WithMessage("Id cannot be null.")
            .Must(BeAValidGuid).WithMessage("Invalid Id format.");
    }
    private bool BeAValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}
