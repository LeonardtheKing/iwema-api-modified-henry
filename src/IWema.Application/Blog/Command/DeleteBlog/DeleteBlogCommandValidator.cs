using FluentValidation;

namespace IWema.Application.Blog.Command.DeleteBlog;


public class DeleteBlogCommandValidator : AbstractValidator<DeleteBlogCommand>
{
    public DeleteBlogCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty().WithMessage("Id is required.")
            .Must(BeAValidGuid).WithMessage("Invalid Id format.")
             .Must(BeAGuid)
            .WithMessage("Invalid GUID format")
            .Must(id => id.ToString().Length == 36).WithMessage("Invalid GUID length. It should be 36 characters.");

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
