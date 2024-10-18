using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Domain.Shared.Error;

namespace PetFamily.Application.Features.Commands.SpeciesManagement.DeleteSpecies;

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