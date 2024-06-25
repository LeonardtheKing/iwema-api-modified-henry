using FluentValidation;
using IWema.Application.Announcements.Command.Update;

namespace IWema.Application.UpcomingEvents.Command.Update;

public class UpdateUpcomingEventCommandValidator : AbstractValidator<UpdateAnnouncementCommand>
{
    public UpdateUpcomingEventCommandValidator()
    {
        // Validate Id
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("The Id must be a valid GUID.");

        // Validate NameOfEvent

        // Validate Date
        RuleFor(x => x.Date)
            .NotEmpty()
             .MinimumLength(5)
            .MaximumLength(50)
            .WithMessage("The Date must be a valid date.");

        When(command => command.File != null, () =>
        {
            RuleFor(command => command.File.Length)
                .LessThanOrEqualTo(1 * 1024 * 1024) // 1MB
                .WithMessage("File size must not exceed 5MB.");
        });


    }

}




