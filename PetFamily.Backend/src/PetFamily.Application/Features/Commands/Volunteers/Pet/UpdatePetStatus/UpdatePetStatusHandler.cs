using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Shared.Error;
using PetFamily.Domain.Shared.IDs;

namespace PetFamily.Application.Features.Commands.Volunteers.Pet.UpdatePetStatus;

public class UpdatePetStatusHandler : ICommandHandler<Guid, UpdatePetStatusCommand>
{
    private IValidator<UpdatePetStatusCommand> _validator;
    private ILogger<UpdatePetStatusHandler> _logger;
    private IVolunteersRepository _repository;

    public UpdatePetStatusHandler(
        IValidator<UpdatePetStatusCommand> validator,
        ILogger<UpdatePetStatusHandler> logger,
        IVolunteersRepository repository)
    {
        _repository = repository;
        _logger = logger;
        _validator = validator;
    }
    
    public async Task<Result<Guid, ErrorList>> Handle(UpdatePetStatusCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToList();

        var getVolunteerResult = await _repository.GetById(command.Id, cancellationToken);
        if (getVolunteerResult.IsFailure)
            return getVolunteerResult.Error.ToErrorList();

        var petResult = getVolunteerResult.Value.Pets.FirstOrDefault(p => p.Id == PetId.Create(command.PetId));
        if (petResult is null)
            return Errors.General.NotFound(command.PetId, "pet").ToErrorList();

        petResult.UpdateStatus(command.Status);

        await _repository.SaveChanges(getVolunteerResult.Value, cancellationToken);

        return command.PetId;
    }
}