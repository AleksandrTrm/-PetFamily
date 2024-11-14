using CSharpFunctionalExtensions;
using Dapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.BreedsManagement.Application.Abstractions;
using PetFamily.Shared.Core.Abstractions;
using PetFamily.Shared.Core.Extensions;
using PetFamily.Shared.SharedKernel.Error;
using PetFamily.Shared.SharedKernel.IDs;

namespace PetFamily.BreedsManagement.Application.Commands.DeleteBreed;

public class DeleteBreedCommandHandler : ICommandHandler<Guid, DeleteBreedCommand>
{
    private ISpeciesRepository _repository;
    private ILogger<DeleteBreedCommandHandler> _logger;
    private IValidator<DeleteBreedCommand> _validator;
    private IReadDbContext _readDbContext;
    private ISqlConnectionFactory _connectionFactory;

    public DeleteBreedCommandHandler(
        IValidator<DeleteBreedCommand> validator,
        ILogger<DeleteBreedCommandHandler> logger,
        ISpeciesRepository repository,
        ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
        _validator = validator;
        _logger = logger;
        _repository = repository;
    }
    
    public async Task<Result<Guid, ErrorList>> Handle(DeleteBreedCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToList();

        var connection = _connectionFactory.Create();

        var sql = """
                    SELECT COUNT(*) FROM pets 
                    WHERE species_breed_species_id = @SpeciesId AND species_breed_breed_id = @BreedId
                  """;

        var parameters = new DynamicParameters();
        parameters.Add("@SpeciesId", command.SpeciesId);
        parameters.Add("@BreedId", command.BreedId);
        
        var petsWithBreedToDeleteCount = await connection.QueryFirstOrDefaultAsync<int>(sql, parameters);
        
        if (petsWithBreedToDeleteCount > 0)
            return Error
                .Failure("breed.was.not.null", "Can not delete breed what used by pet")
                .ToErrorList();

        var speciesResult = await _repository.GetSpeciesById(command.SpeciesId, cancellationToken);
        if (speciesResult.IsFailure)
            return speciesResult.Error.ToErrorList();

        var breedToDelete = speciesResult.Value.Breeds.First(b => b.Id == BreedId.Create(command.BreedId));
        speciesResult.Value.RemoveBreed(breedToDelete);

        await _repository.SaveChanges(speciesResult.Value, cancellationToken);
        
        return command.BreedId;
    }
}