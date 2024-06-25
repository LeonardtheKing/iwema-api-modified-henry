using FluentValidation;

namespace IWema.Application.SideMenu.ChildSideMenu.Command.Add;

public class AddChildSideMenuCommandValidator : AbstractValidator<AddChildSideMenuCommand>
{
    public AddChildSideMenuCommandValidator()
    {
        // Validate MainMenuId
        RuleFor(command => command.ParentToggleId)
            .NotEmpty().WithMessage("Main menu ID is required.");

        // Validate Name
        RuleFor(command => command.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

        // Validate Link
        RuleFor(command => command.Link)
            .NotEmpty().WithMessage("Link is required.")
            .MaximumLength(200).WithMessage("Link must not exceed 200 characters.")
            .Must(BeAValidUrl).WithMessage("Link must be a valid URL.");

        // Validate Icon
        RuleFor(command => command.Icon)
            .NotEmpty().WithMessage("Icon is required.")
            .MaximumLength(100).WithMessage("Icon must not exceed 100 characters.");
    }

    // Custom validator method for URL validation
    private bool BeAValidUrl(string url)
    {
        Uri resultUri;
        return Uri.TryCreate(url, UriKind.Absolute, out resultUri)
            && (resultUri.Scheme == Uri.UriSchemeHttp || resultUri.Scheme == Uri.UriSchemeHttps);
    }
}
