using FluentValidation;
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;

namespace IWema.Application.Announcements.Command.Add
{
    public class AddAnnouncementCommandValidator : AbstractValidator<AddAnnouncementCommand>
    {
        public AddAnnouncementCommandValidator()
        {
            RuleFor(x => x.File)
                .NotNull().WithMessage("File is required.");

            RuleFor(x => x.File)
                .Must(BeAValidImage).WithMessage("File must be a valid image.")
                .When(x => x.File != null); // Only validate if File is provided

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .Must(NotContainSpecialCharacters).WithMessage("Invalid characters.");

            RuleFor(x => x.Date)
                .NotEmpty().WithMessage("Date is required.")
                .Must(NotContainSpecialCharacters).WithMessage("Invalid characters.");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Content is required.")
                .Must(NotContainSpecialCharacters).WithMessage("Invalid characters.");
               
        }

        private bool BeAValidImage(IFormFile file)
        {
            if (file == null)
                return true; // Allow null, as this is validated by another rule

            var allowedExtensions = new[] { ".png", ".jpg", ".jpeg" };
            var fileExtension = System.IO.Path.GetExtension(file.FileName).ToLower();
            return allowedExtensions.Contains(fileExtension);
        }

        private bool NotContainSpecialCharacters(string title)
        {
            if (string.IsNullOrEmpty(title))
                return true; // Allow null or empty, as this is validated by another rule

            // Regex to allow only letters, numbers, spaces, and basic punctuation
            var regex = new Regex(@"^[a-zA-Z0-9\s]*$");
            return regex.IsMatch(title);
        }

    }
}
