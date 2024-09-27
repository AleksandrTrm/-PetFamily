using FluentValidation;
using PetFamily.Application.DTOs.VolunteerDtos;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared.Error;
using PetFamily.Domain.VolunteersManagement.AggregateRoot;
using PetFamily.Domain.VolunteersManagement.ValueObjects.Shared;
using PetFamily.Domain.VolunteersManagement.ValueObjects.Volunteer;

namespace PetFamily.Application.Features.Commands.Volunteers.Update.UpdateMainInfo;

public class UpdateMainInfoCommandValidator : AbstractValidator<UpdateMainInfoCommand>
{
    public UpdateMainInfoCommandValidator()
    {
        RuleFor(r => r.Id)
            .NotEmpty()
            .WithError(Errors.General.InvalidValue());
    } 
}

public class UpdateMainInfoDtoValidator : AbstractValidator<UpdateVolunteerMainInfoDto>
{
    public UpdateMainInfoDtoValidator()
    {
        RuleFor(r => r.FullName)
            .MustBeValueObject(n => FullName.Create(n.Name, n.Surname, n.Patronymic));

        RuleFor(r => r.Experience)
            .Must(e => e is > 0 and <= Volunteer.MAX_EXPERIENCE_YEARS)
            .WithError(Errors.General.InvalidCount(0, "experience", Volunteer.MAX_EXPERIENCE_YEARS));

        RuleFor(r => r.Description)
            .MustBeValueObject(Description.Create);

        RuleFor(r => r.PhoneNumber)
            .MustBeValueObject(PhoneNumber.Create);
    }
}