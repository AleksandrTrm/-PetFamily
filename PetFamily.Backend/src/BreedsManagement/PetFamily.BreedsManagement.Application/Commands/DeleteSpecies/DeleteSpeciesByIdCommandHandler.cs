using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.BreedsManagement.Application.Abstractions;
using PetFamily.Shared.Core.Abstractions;
using PetFamily.Shared.SharedKernel.Error;

namespace PetFamily.BreedsManagement.Application.Commands.DeleteSpecies;

public class DeleteSpeciesByIdCommandHandler : ICommandHandler<Guid, DeleteSpeciesByIdCommand>
{
    private ILogger<DeleteSpeciesByIdCommandHandler> _logger;
    private ISpeciesRepository _repository;

    public DeleteSpeciesByIdCommandHandler(
        ILogger<DeleteSpeciesByIdCommandHandler> logger,
        ISpeciesRepository repository)
    {
        _repository = repository;
        _logger = logger;
    }
    
    public async Task<Result<Guid, ErrorList>> Handle(DeleteSpeciesByIdCommand byIdCommand, CancellationToken cancellationToken)
    {
        if (byIdCommand.SpeciesId == Guid.Empty)
            return Errors.General.InvalidValue("speciesId").ToErrorList();
        
        var getSpeciesById = await _repository
            .GetSpeciesById(byIdCommand.SpeciesId, cancellationToken);
        if (getSpeciesById.IsFailure)
            return byIdCommand.SpeciesId;

        await _repository.DeleteSpeciesById(byIdCommand.SpeciesId, cancellationToken);

        return byIdCommand.SpeciesId;
    }
}