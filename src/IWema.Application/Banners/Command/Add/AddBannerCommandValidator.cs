using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace IWema.Application.Banners.Command.Add;

public class AddBannerCommandValidator : AbstractValidator<AddBannerCommand>
{
    public AddBannerCommandValidator()
    {
        RuleFor(x => x.File)
            .Must(BeAValidFileRef).WithMessage("This is an invalid file")
            .NotEmpty();

        RuleFor(x => x.File)
            .Must(BeAValidMediaFile).WithMessage("This is an invalid file type")
            .NotEmpty();
    }

    public static bool BeAValidLink(string value)
    {
        if (!value.Contains("http") || !value.Contains("https"))
            return false;

        return true;
    }

    public static bool BeAValidFileRef(IFormFile file)
    {
        if (file.Length < 1)
            return false;

        return true;
    }

    public static bool BeAValidMediaFile(IFormFile file)
    {
        var fileExt = file.FileName.Split('.')[1].ToLower();
        string[] allowedMediaExt = ["jpeg", "jpg", "gif", "tiff", "psd", "pdf", "eps", "ai", "indd", "raw", "png", "bmp"];

        if (allowedMediaExt.Contains(fileExt))
            return true;

        return false;
    }

    
}