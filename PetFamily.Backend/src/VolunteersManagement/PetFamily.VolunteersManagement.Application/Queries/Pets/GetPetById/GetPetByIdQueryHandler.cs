using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Shared.Core.Abstractions;
using PetFamily.Shared.Core.DTOs.Pets;
using PetFamily.Shared.SharedKernel.Error;
using PetFamily.VolunteersManagement.Application.Abstractions;

namespace PetFamily.VolunteersManagement.Application.Queries.Pets.GetPetById;

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