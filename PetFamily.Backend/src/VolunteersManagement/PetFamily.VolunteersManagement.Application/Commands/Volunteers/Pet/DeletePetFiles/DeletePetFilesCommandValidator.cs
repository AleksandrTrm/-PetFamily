using FluentValidation;
using PetFamily.Shared.Core.Validation;
using PetFamily.Shared.SharedKernel.Error;

namespace PetFamily.VolunteersManagement.Application.Commands.Volunteers.Pet.DeletePetFiles;

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