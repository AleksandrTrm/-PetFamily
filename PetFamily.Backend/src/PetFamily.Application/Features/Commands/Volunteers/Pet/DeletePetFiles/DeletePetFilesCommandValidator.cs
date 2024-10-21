using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared.Error;

namespace PetFamily.Application.Features.Commands.Volunteers.Pet.DeletePetFiles;

public class DeletePetFilesCommandValidator : AbstractValidator<DeletePetFilesCommand>
{
    public DeletePetFilesCommandValidator()
    {
        RuleFor(c => c.VolunteerId)
            .NotEmpty()
            .WithError(Errors.General.InvalidValue(invalidField: "volunteerId"));
        
        RuleFor(c => c.PetId)
            .NotEmpty()
            .WithError(Errors.General.InvalidValue(invalidField: "petId"));

        RuleForEach(c => c.FilesNames)
            .NotEmpty()
            .WithError(Errors.General.InvalidValue(invalidField: "filesNames"));

    }
}