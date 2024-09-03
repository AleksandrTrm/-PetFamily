using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.Shared;
using PetFamily.Domain.ValueObjects;
using PetFamily.Domain.VolunteersManagement.Volunteer.VolunteerValueObjects;

namespace PetFamily.Application.Volunteers.Update.UpdateMainInfo;

public class UpdateMainInfoHandler
{
    private IVolunteersRepository _repository;
    private ILogger<UpdateMainInfoHandler> _logger;

    public UpdateMainInfoHandler(
        IVolunteersRepository repository,
        ILogger<UpdateMainInfoHandler> logger)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task<Result<Guid, Error>> Handle(
        UpdateMainInfoRequest request,
        CancellationToken cancellationToken = default)
    {
        var volunteerResult = await _repository.GetById(request.Id, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error;

        var fullNameDto = request.UpdateVolunteerMainInfoDto.FullName;
        var fullName = FullName.Create(
            fullNameDto.Name, 
            fullNameDto.Surname, 
            fullNameDto.Patronymic).Value;

        var experience = request.UpdateVolunteerMainInfoDto.Experience;

        var description = Description.Create(request.UpdateVolunteerMainInfoDto.Description).Value;

        var phoneNumber = PhoneNumber.Create(request.UpdateVolunteerMainInfoDto.PhoneNumber).Value;
        
        volunteerResult.Value.UpdateMainInfo(fullName, experience, description, phoneNumber);

        await _repository.Update(volunteerResult.Value, cancellationToken);
        
        _logger.LogInformation("Main info of volunteer with {id} has been updated", request.Id);
        
        return (Guid)volunteerResult.Value.Id;
    }
}