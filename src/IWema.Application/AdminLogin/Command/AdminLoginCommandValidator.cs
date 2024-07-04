namespace IWema.Application.Login.AdminLogin.Command;

using FluentValidation;

public class AdminLoginCommandValidator : AbstractValidator<AdminLoginCommand>
{
    public AdminLoginCommandValidator()
    {
        // Rule for Email
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.");

        // Rule for Password
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
    }
}

