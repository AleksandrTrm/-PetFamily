using PetFamily.Domain.Shared;
using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared.IDs;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.ValueObjects;

namespace PetFamily.Application.Volunteers.Update.UpdateRequisites;

public class UpdateRequisitesHandler
{
    private IVolunteersRepository _repository;
    private ILogger<UpdateRequisitesHandler> _logger;

    public UpdateRequisitesHandler(IVolunteersRepository repository, ILogger<UpdateRequisitesHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> Handle(
        UpdateRequisitesRequest request, 
        CancellationToken cancellationToken = default)
    {
        var volunteerResult = await _repository.GetById(request.VolunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error;

        List<Requisite> requisites = [];
        foreach (var requisite in request.Requisites.Requisites)
        {
            var description = Description.Create(requisite.Description).Value;
            
            requisites.Add(Requisite.Create(requisite.Title, description).Value);
        }

        var requisitesToUpdate = Requisites.Create(requisites);
        if (requisitesToUpdate.IsFailure)
            return requisitesToUpdate.Error;
        
        volunteerResult.Value.UpdateRequisites(requisitesToUpdate.Value);

        var updateResult = await _repository.Update(volunteerResult.Value, cancellationToken);
        
        _logger.LogInformation("Requisites of volunteer with {id} has been updated", request.VolunteerId);

        return request.VolunteerId;
    }
}