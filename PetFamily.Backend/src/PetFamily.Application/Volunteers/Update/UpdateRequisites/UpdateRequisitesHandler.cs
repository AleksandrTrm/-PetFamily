using PetFamily.Domain.Shared;
using CSharpFunctionalExtensions;
using FluentValidation;
using PetFamily.Domain.Shared.IDs;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Extensions;
using PetFamily.Domain.ValueObjects;

namespace PetFamily.Application.Volunteers.Update.UpdateRequisites;

public class UpdateRequisitesHandler
{
    private readonly IVolunteersRepository _repository;
    private readonly ILogger<UpdateRequisitesHandler> _logger;
    private readonly IValidator<UpdateRequisitesCommand> _validator;

    public UpdateRequisitesHandler(
        IVolunteersRepository repository,
        IValidator<UpdateRequisitesCommand> validator,
        ILogger<UpdateRequisitesHandler> logger)
    {
        _validator = validator;
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        UpdateRequisitesCommand command, 
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToList();
        
        var volunteerResult = await _repository.GetById(command.VolunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();

        List<Requisite> requisites = [];
        foreach (var requisite in command.Requisites)
        {
            var description = Description.Create(requisite.Description).Value;
            
            requisites.Add(Requisite.Create(requisite.Title, description).Value);
        }

        var requisitesToUpdate = new ValueObjectList<Requisite>(requisites);
        
        volunteerResult.Value.UpdateRequisites(requisitesToUpdate);

        await _repository.Save(volunteerResult.Value, cancellationToken);
        
        _logger.LogInformation("Requisites of volunteer with {id} has been updated", command.VolunteerId);

        return command.VolunteerId;
    }
}