using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Shared.Error;
using PetFamily.Domain.Shared.IDs;

namespace PetFamily.Application.Features.Commands.Volunteers.Pet.DeletePet;

public class DeletePetHandler : ICommandHandler<Guid, DeletePetCommand>
{
    private IValidator<DeletePetCommand> _validator;
    private ILogger<DeletePetHandler> _logger;
    private IVolunteersRepository _repository;

    public DeletePetHandler(
        IValidator<DeletePetCommand> validator,
        ILogger<DeletePetHandler> logger,
        IVolunteersRepository repository)
    {
        _repository = repository;
        _logger = logger;
        _validator = validator;
    }
    
    public async Task<Result<Guid, ErrorList>> Handle(DeletePetCommand command, CancellationToken cancellationToken)
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

        volunteerResult.Value.DeletePet(pet);

        await _repository.SaveChanges(volunteerResult.Value, cancellationToken);

        _logger.LogInformation("Pet with id - '{}' has been deleted", command.PetId);
        
        return command.PetId;
    }
}