using FluentValidation;

namespace IWema.Application.ManagementTeam.Command.Update
{
    public class UpdateManagementTeamCommandValidator : AbstractValidator<UpdateManagementTeamCommand>
    {
        public UpdateManagementTeamCommandValidator()
        {
            // Ensure ID is not empty
            RuleFor(command => command.Id).NotEmpty();

            // Ensure ManagementTeamId is not empty
            RuleFor(command => command.ManagementTeamId).NotEmpty();

            // NameOfExecutive must not be empty and has a max length of 200 characters
            RuleFor(command => command.NameOfExecutive)
                .NotEmpty()
                .MaximumLength(200);

            // Position must not be empty and has a max length of 100 characters
            RuleFor(command => command.Position)
                .NotEmpty()
                .MaximumLength(100);

            // ImageLink must be a valid URL if it's not empty
          

            // Example of file size validation (e.g., not more than 5MB)
            When(command => command.File != null, () =>
            {
                RuleFor(command => command.File.Length)
                    .LessThanOrEqualTo(1 * 1024 * 1024) // 1MB
                    .WithMessage("File size must not exceed 5MB.");
            });

            // Quote can have a custom validation, here just ensuring it's not too long
            RuleFor(command => command.Quote)
                .MaximumLength(500);

            // ProfileLink must be a valid URL if provided
            When(command => !string.IsNullOrEmpty(command.ProfileLink), () =>
            {
                RuleFor(command => command.ProfileLink)
                    .Must(Link => Uri.TryCreate(Link, UriKind.Absolute, out var uriResult)
                        && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps))
                    .WithMessage("ProfileLink must be a valid URL.");
            });
        }
    }
}
