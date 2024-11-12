using FluentValidation;
using PetFamily.Shared.Core.Validation;
using PetFamily.Shared.SharedKernel.Error;

namespace PetFamily.BreedsManagement.Application.Commands.DeleteBreed;

public class DeleteBreedCommandValidator : AbstractValidator<DeleteBreedCommand>
{
    public DeleteBreedCommandValidator()
    {
        RuleFor(c => c.SpeciesId)
            .NotNull()
            .WithError(Errors.General.InvalidValue());
        
        RuleFor(c => c.BreedId)
            .NotNull()
            .WithError(Errors.General.InvalidValue());
    }
}