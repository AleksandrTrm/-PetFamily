using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Shared.Error;
using PetFamily.Domain.Shared.IDs;
using PetFamily.Domain.SpeciesManagement.Entitites;
using PetFamily.Domain.SpeciesManagement.ValueObjects;

namespace PetFamily.Application.Features.Commands.SpeciesManagement.CreateBreed;

public class CreateBreedCommandHandler : ICommandHandler<Guid, CreateBreedCommand>
{
    private ILogger<CreateBreedCommandHandler> _logger;
    private ISpeciesRepository _repository;
    private CreateBreedCommandValidator _validator;

    public CreateBreedCommandHandler(
        ILogger<CreateBreedCommandHandler> logger,
        ISpeciesRepository repository,
        CreateBreedCommandValidator validator)
    {
        _validator = validator;
        _repository = repository;
        _logger = logger;
    }
    
    public async Task<Result<Guid, ErrorList>> Handle(CreateBreedCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToList();

        var breed = new Breed(BreedId.NewBreedId(), BreedValue.Create(command.Breed).Value);

        var createBreedResult = await _repository
            .CreateBreed(SpeciesId.Create(command.SpeciesId), breed, cancellationToken);
        if (createBreedResult.IsFailure)
            return createBreedResult.Error.ToErrorList();

        return createBreedResult.Value;
    }
}