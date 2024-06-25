﻿using FluentValidation;

namespace IWema.Application.Login.Command
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        //public LoginCommandValidator()
        //{
        //    RuleFor(x => x.Email)
        //        .NotEmpty().WithMessage("Email is required.")
        //        .EmailAddress().WithMessage("A valid email address is required.");

        //    RuleFor(x => x.Password)
        //        .NotEmpty().WithMessage("Password is required.")
        //        .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
        //}

        public LoginCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
        }

    }
}
