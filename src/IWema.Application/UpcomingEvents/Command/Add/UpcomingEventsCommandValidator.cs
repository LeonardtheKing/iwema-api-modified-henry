using FluentValidation;

namespace IWema.Application.UpcomingEvents.Command.Add;

public class UpcomingEventsCommandValidator : AbstractValidator<AddUpcomingEventsCommand>
{
    public UpcomingEventsCommandValidator()
    {
        // File validation
        RuleFor(command => command.File)
             .NotNull()
             .WithMessage("Please select a file to upload.")
             .DependentRules(() =>
             {
                 RuleFor(command => command.File)
                     .Must(file => file.Length > 0)
                     .WithMessage("The selected file is empty. Please select a file with content.")
                     .Must(file => IsValidFileSize(file.Length, 1 * 1024 * 1024)) // 1 MB limit
                     .WithMessage("The selected file is too large. Please keep it under 1 MB.")
                     .Must(file => IsValidExtension(file.FileName))
                     .WithMessage("Invalid file type. Only image files (JPG, JPEG, PNG) are allowed.");
             });

        // NameOfEvent validation
        RuleFor(command => command.NameOfEvent)
            .NotEmpty()
            .WithMessage("Please provide a name for the event.")
            .Length(2, 255)
            .WithMessage("The event name must be between 2 and 255 characters.");

        // Date validation
        RuleFor(command => command.Date)
            .NotEmpty()
            .WithMessage("Please specify the date of the event.");

        bool IsValidFileSize(long size, long maxSize)
        {
            return size <= maxSize;
        }

        bool IsValidExtension(string fileName)
        {
            var allowedExtensions = new HashSet<string> { ".jpg", ".jpeg", ".png" };
            var fileExtension = Path.GetExtension(fileName).ToLowerInvariant();
            return allowedExtensions.Contains(fileExtension);
        }

    }
}
