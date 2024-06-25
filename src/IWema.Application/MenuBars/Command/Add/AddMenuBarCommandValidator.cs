using FluentValidation;

namespace IWema.Application.MenuBars.Command.Add;

public class AddMenuBarCommandValidator : AbstractValidator<AddMenuBarCommand>
{
    public AddMenuBarCommandValidator()
    {
        RuleFor(x => x.Name)
            .MinimumLength(1)
            .NotEmpty();

        RuleFor(x => x.Link)
            .Must(BeAValiLink).WithMessage("This is an invalid link")
            .NotEmpty();
    }

    public static bool BeAValiLink(string value)
    {
        if (!value.Contains("http") || !value.Contains("https"))
            return false;

        return true;
    }
}