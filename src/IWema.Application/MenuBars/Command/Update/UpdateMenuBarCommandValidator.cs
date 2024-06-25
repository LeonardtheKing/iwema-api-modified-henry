using FluentValidation;

namespace IWema.Application.MenuBars.Command.Update;

public class UpdateMenuBarCommandValidator : AbstractValidator<UpdateMenuBarCommand>
{
    public UpdateMenuBarCommandValidator()
    {
        // Validates that the Name is not empty and meets specific criteria
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .Length(2, 50).WithMessage("Name must be between 2 and 50 characters long")
            .Matches("^[a-zA-Z]").WithMessage("Name must contain only letters");
        // Validates that the Link is not empty, and could also add format validation
        RuleFor(x => x.Link)
            .NotEmpty().WithMessage("Link is required")
            .Must(BeAValidUrl).WithMessage("Link must be a valid URL");

        // Validates that the Description is not empty and meets length requirements
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .Length(10, 200).WithMessage("Description must be between 10 and 200 characters long");
    }

    private bool BeAValidUrl(string link)
    {
        return Uri.TryCreate(link, UriKind.Absolute, out var uriResult)
               && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }

}