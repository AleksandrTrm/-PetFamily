using FluentValidation;
using PetFamily.Shared.Core.Validation;
using PetFamily.Shared.SharedKernel.DTOs.VolunteerDtos;
using PetFamily.Shared.SharedKernel.Error;
using PetFamily.Shared.SharedKernel.ValueObjects.Volunteers.Shared;
using PetFamily.Shared.SharedKernel.ValueObjects.Volunteers.Volunteer;
using PetFamily.VolunteersManagement.Domain.AggregateRoot;

namespace PetFamily.VolunteersManagement.Application.Commands.Volunteers.Update.UpdateMainInfo;

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