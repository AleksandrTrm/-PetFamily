using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Shared.Core.Abstractions;
using PetFamily.Shared.Core.Extensions;
using PetFamily.Shared.SharedKernel.Error;
using PetFamily.Shared.SharedKernel.IDs;
using PetFamily.VolunteersManagement.Application.Abstractions;

namespace PetFamily.VolunteersManagement.Application.Commands.Volunteers.Pet.UpdatePetStatus;

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

        var volunteerResult = await _repository.GetById(command.Id, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();

        var pet = volunteerResult.Value.Pets.FirstOrDefault(p => p.Id == PetId.Create(command.PetId));
        if (pet is null)
            return Errors.General.NotFound(command.PetId, "pet").ToErrorList();

        pet.UpdateStatus(command.Status);

        await _repository.SaveChanges(volunteerResult.Value, cancellationToken);

        return command.PetId;
    }
}