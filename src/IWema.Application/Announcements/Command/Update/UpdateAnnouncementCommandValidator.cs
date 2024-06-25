using FluentValidation;

namespace IWema.Application.Announcements.Command.Update;

public class UpdateAnnouncementCommandValidator: AbstractValidator<UpdateAnnouncementCommandInputModel>
{
    public UpdateAnnouncementCommandValidator()
    {
        RuleFor(command => command.AnnouncementId)
          .NotEmpty().WithMessage("AnnouncementId is required.")
          .Must(BeAValidGuid).WithMessage("Invalid AnnouncementId format.")
           .Must(BeAGuid)
          .WithMessage("Invalid GUID format")
          .Must(id => id.ToString().Length == 36).WithMessage("Invalid GUID length. It should be 36 characters."); ;

        RuleFor(command => command.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.");

        RuleFor(command => command.Date)
            .NotEmpty().WithMessage("Date is required.")
            .MaximumLength(100).WithMessage("Date cannot exceed 100 characters.");


        RuleFor(command => command.Content)
            .NotEmpty().WithMessage("Content is required.")
            .MaximumLength(100).WithMessage("Content cannot exceed 100 characters.");

        // Example of file size validation (e.g., not more than 5MB)
        When(command => command.File != null, () =>
        {
            RuleFor(command => command.File.Length)
                .LessThanOrEqualTo(1 * 1024 * 1024) // 1MB
                .WithMessage("File size must not exceed 5MB.");
        });

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
