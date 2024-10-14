using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Shared.Error;
using PetFamily.Domain.Shared.IDs;
using PetFamily.Domain.SpeciesManagement.AggregateRoot;
using PetFamily.Domain.SpeciesManagement.ValueObjects;

namespace PetFamily.Application.Features.Commands.SpeciesManagement.CreateSpecies;

public class CreateSpeciesCommandHandler : ICommandHandler<Guid, CreateSpeciesCommand>
{
    private ILogger<CreateSpeciesCommandHandler> _logger;
    private ISpeciesRepository _repository;
    private CreateSpeciesCommandValidator _validator;

    public CreateSpeciesCommandHandler(
        ILogger<CreateSpeciesCommandHandler> logger,
        ISpeciesRepository repository,
        CreateSpeciesCommandValidator validator)
    {
        _validator = validator;
        _repository = repository;
        _logger = logger;
    }
    
    public async Task<Result<Guid, ErrorList>> Handle(CreateSpeciesCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToList();

        var getSpeciesResult = await _repository.GetSpeciesByName(command.Species);
        if (getSpeciesResult.IsSuccess)
            return Errors.General.AlreadyExists().ToErrorList();
        
        var species = new Species(SpeciesId.NewSpeciesId(), command.Species);

        var createSpeciesResult = await _repository.CreateSpecies(species, cancellationToken);
        if (createSpeciesResult.IsFailure)
            return createSpeciesResult.Error.ToErrorList();

        return createSpeciesResult.Value;
    }
}