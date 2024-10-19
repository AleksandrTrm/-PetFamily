using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared.Error;

namespace PetFamily.Application.Features.Commands.SpeciesManagement.DeleteBreed;

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