using CSharpFunctionalExtensions;
using Dapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.DTOs.Species;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Shared.Error;

namespace PetFamily.Application.Features.Commands.SpeciesManagement.DeleteBreed;

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

        var deleteBreedByIdResult = await _repository
            .DeleteBreedById(command.SpeciesId, command.BreedId, cancellationToken);
        if (deleteBreedByIdResult.IsFailure)
            return deleteBreedByIdResult.Error.ToErrorList();
        
        return command.BreedId;
    }
}