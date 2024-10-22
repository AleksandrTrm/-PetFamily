using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.DTOs.Pets;
using PetFamily.Domain.Shared.Error;

namespace PetFamily.Application.Features.Queries.Pets.GetPetById;

public class GetPetByIdQueryHandler : IQueryHandler<Result<PetDto, ErrorList>, GetPetByIdQuery>
{
    private IReadDbContext _readDbContext;
    private ILogger<GetPetByIdQueryHandler> _logger;

    public GetPetByIdQueryHandler(
        ILogger<GetPetByIdQueryHandler> logger,
        IReadDbContext readDbContext)
    {
        _logger = logger;
        _readDbContext = readDbContext;
    }
    
    public async Task<Result<PetDto, ErrorList>> Handle(GetPetByIdQuery query, CancellationToken cancellationToken)
    {
        var petResult = await _readDbContext.Pets.FirstOrDefaultAsync(p => p.Id == query.Id, cancellationToken);
        if (petResult is null)
            return Errors.General.NotFound(query.Id, "pet").ToErrorList();

        _logger.LogInformation("Requested pet with id - '{id}'", query.Id);
        
        return petResult;
    }
}