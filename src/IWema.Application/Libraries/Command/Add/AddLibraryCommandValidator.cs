using FluentValidation;
using IWema.Application.Common.DTO;
using Microsoft.AspNetCore.Http;

namespace IWema.Application.Libraries.Command.Add
{
    public class AddLibraryCommandValidator : AbstractValidator<AddLibraryCommand>
    {
        public AddLibraryCommandValidator()
        {
            RuleFor(x => x.Type)
                .Must(BaAValidLibraryType).WithMessage("This is an invalid type")
                .NotEmpty();

            RuleFor(x => x.Description)
                .NotEmpty();

            //RuleFor(x => x.File)
            //    .Must(BeAValidFileRef).WithMessage("This is an invalid file")
            //    .NotEmpty();

            RuleFor(x => x.File)
                .Must(BeAValidMediaFile).WithMessage("This is an invalid file")
                .NotEmpty();
        }

        public static bool BaAValidLibraryType(string type)
        {
            if (Enum.IsDefined(typeof(LibraryTypeEnum), type))
                return true;
            return false;
        }

        public static bool BeAValidFileRef(IFormFile file)
        {
            if (file.Length < 1)
                return false;

            return true;
        }


        private bool BeAValidMediaFile(IFormFile file)
        {
            if (file == null)
                return true; // Allow null, as this is validated by another rule

            var allowedExtensions = new[] { ".png", ".jpg", ".jpeg", ".pdf", ".doc", ".xls", ".txt", ".ppt",".pptx", ".docx",".xlsx" };
            var fileExtension = System.IO.Path.GetExtension(file.FileName).ToLower();
            return allowedExtensions.Contains(fileExtension);
        }

    }
}
