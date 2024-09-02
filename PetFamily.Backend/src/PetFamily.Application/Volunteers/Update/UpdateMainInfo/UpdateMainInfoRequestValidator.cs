using FluentValidation;
using PetFamily.Domain.Shared;
using PetFamily.Domain.ValueObjects;
using PetFamily.Application.Validation;
using PetFamily.Domain.VolunteersManagement.Volunteer;
using PetFamily.Domain.VolunteersManagement.Volunteer.VolunteerValueObjects;

namespace PetFamily.Application.Volunteers.Update.UpdateMainInfo;

public class UpdateMainInfoRequestValidator : AbstractValidator<UpdateMainInfoRequest>
{
    public UpdateMainInfoRequestValidator()
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