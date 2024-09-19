using FluentValidation;
using FluentValidation.Results;
using PetFamily.Domain.Shared;
using PetFamily.Application.Validation;

namespace PetFamily.Application.Volunteers.Pet.UploadPetFiles;

public class UploadPetFilesCommandValidator : AbstractValidator<UploadPetFilesCommand>
{
    public UploadPetFilesCommandValidator()
    {
        RuleFor(r => r.VolunteerId)
            .NotEmpty()
            .WithError(Errors.General.InvalidValue("volunteerId"));
        
        RuleFor(r => r.PetId)
            .NotEmpty()
            .WithError(Errors.General.InvalidValue("petId"));

        RuleFor(r => r.Files)
            .NotEmpty()
            .WithError(Errors.General.InvalidValue("files"));

        RuleForEach(r => r.Files)
            .SetValidator(new PetFileValidatorValidator());
    }

    public class PetFileValidatorValidator : AbstractValidator<UploadFileCommand>
    {
        public const int MAX_FILE_SIZE = 10 * 1024 * 1024;

        public PetFileValidatorValidator()
        {
            RuleFor(p => p.Content.Length)
                .Must(x => x <= MAX_FILE_SIZE)
                .WithError(Errors.General.InvalidValue("file size"));

            //RuleFor(p => p.FileName)
            //    .Must(name =>
            //    {
            //        var extension = Path.GetExtension(name);
            //        
            //        return Constants.ALLOWED_IMAGES_EXTENSIONS.Contains(name);
            //    })
            //    .WithError(Errors.General.InvalidValue("file type"));
        }
    }
}