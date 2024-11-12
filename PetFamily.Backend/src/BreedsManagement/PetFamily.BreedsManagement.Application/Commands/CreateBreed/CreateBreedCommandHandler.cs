using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.BreedsManagement.Application.Abstractions;
using PetFamily.BreedsManagement.Domain.Entitites;
using PetFamily.Shared.Core.Abstractions;
using PetFamily.Shared.Core.Extensions;
using PetFamily.Shared.SharedKernel.Error;
using PetFamily.Shared.SharedKernel.IDs;

namespace PetFamily.BreedsManagement.Application.Commands.CreateBreed;

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

        var getBreedResult = await _repository.GetBreedByName(command.SpeciesId, command.Breed);
        if (getBreedResult.IsSuccess)
            return Errors.General.AlreadyExists().ToErrorList();
        
        var breed = new Breed(BreedId.NewBreedId(), command.Breed);

        var createBreedResult = await _repository
            .CreateBreed(SpeciesId.Create(command.SpeciesId), breed, cancellationToken);
        if (createBreedResult.IsFailure)
            return createBreedResult.Error.ToErrorList();

        return createBreedResult.Value;
    }
}