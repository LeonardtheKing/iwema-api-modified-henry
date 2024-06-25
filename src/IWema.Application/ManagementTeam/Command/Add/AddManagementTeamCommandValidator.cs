using FluentValidation;
using IWema.Application.ManagementTeam.Command.Add;
using Microsoft.AspNetCore.Http;

public class AddManagementTeamCommandValidator : AbstractValidator<AddManagementTeamCommand>
{
    public AddManagementTeamCommandValidator()
    {
        RuleFor(x => x.File)
            .NotNull().WithMessage("File is required.");

        RuleFor(x => x.File)
            .Must(BeAValidImage).WithMessage("File must be a valid image.")
            .When(x => x.File != null); // Only validate if File is provided

        RuleFor(x => x.nameOfExecutive)
            .NotEmpty().WithMessage("Name of executive is required.");

        RuleFor(x => x.position)
            .NotEmpty().WithMessage("Position is required.");

        RuleFor(x => x.quote)
            .NotEmpty().WithMessage("Quote is required.");

        RuleFor(x => x.profileLink)
            .NotEmpty().WithMessage("Profile link is required.")
            .Must(BeAValidLink).WithMessage("Invalid profile link format.");
    }

    private bool BeAValidImage(IFormFile file)
    {
        if (file == null)
            return true; // Allow null, as this is validated by another rule

        var allowedExtensions = new[] { ".png", ".jpg", ".jpeg" };
        var fileExtension = System.IO.Path.GetExtension(file.FileName).ToLower();
        return allowedExtensions.Contains(fileExtension);
    }

    private bool BeAValidLink(string profileLink)
    {
        Uri uriResult;
        return Uri.TryCreate(profileLink, UriKind.Absolute, out uriResult)
            && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }
}
