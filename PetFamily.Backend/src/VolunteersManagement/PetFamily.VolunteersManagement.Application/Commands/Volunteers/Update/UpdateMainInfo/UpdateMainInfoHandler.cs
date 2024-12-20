﻿using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Shared.Core.Abstractions;
using PetFamily.Shared.Core.Extensions;
using PetFamily.Shared.SharedKernel.Error;
using PetFamily.Shared.SharedKernel.ValueObjects.Volunteers.Shared;
using PetFamily.Shared.SharedKernel.ValueObjects.Volunteers.Volunteer;
using PetFamily.VolunteersManagement.Application.Abstractions;

namespace PetFamily.VolunteersManagement.Application.Commands.Volunteers.Update.UpdateMainInfo;

public class UpdateMainInfoHandler : ICommandHandler<Guid, UpdateMainInfoCommand>
{
    private readonly IVolunteersRepository _repository;
    private readonly ILogger<UpdateMainInfoHandler> _logger;
    private IValidator<UpdateMainInfoCommand> _validator;

    public UpdateMainInfoHandler(
        IVolunteersRepository repository,
        IValidator<UpdateMainInfoCommand> validator,
        ILogger<UpdateMainInfoHandler> logger)
    {
        _validator = validator;
        _logger = logger;
        _repository = repository;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        UpdateMainInfoCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToList();
        
        var volunteerResult = await _repository.GetById(command.Id, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();

        var fullNameDto = command.UpdateVolunteerMainInfoDto.FullName;
        var fullName = FullName.Create(
            fullNameDto.Name, 
            fullNameDto.Surname, 
            fullNameDto.Patronymic).Value;

        var experience = command.UpdateVolunteerMainInfoDto.Experience;

        var description = Description.Create(command.UpdateVolunteerMainInfoDto.Description).Value;

        var phoneNumber = PhoneNumber.Create(command.UpdateVolunteerMainInfoDto.PhoneNumber).Value;
        
        volunteerResult.Value.UpdateMainInfo(fullName, experience, description, phoneNumber);

        await _repository.SaveChanges(volunteerResult.Value, cancellationToken);
        
        _logger.LogInformation("Main info of volunteer with {id} has been updated", command.Id);
        
        return (Guid)volunteerResult.Value.Id;
    }
}