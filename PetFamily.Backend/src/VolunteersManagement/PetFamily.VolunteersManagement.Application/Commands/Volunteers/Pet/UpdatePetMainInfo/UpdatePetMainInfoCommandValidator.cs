using FluentValidation;
using PetFamily.Shared.Core.Validation;
using PetFamily.Shared.SharedKernel.Error;
using PetFamily.Shared.SharedKernel.ValueObjects.Volunteers.Pets;
using PetFamily.Shared.SharedKernel.ValueObjects.Volunteers.Shared;

namespace PetFamily.VolunteersManagement.Application.Commands.Volunteers.Pet.UpdatePetMainInfo;

public class UpdatePetMainInfoCommandValidator : AbstractValidator<UpdatePetMainInfoCommand>
{
    public UpdatePetMainInfoCommandValidator()
    {
        RuleFor(r => r.VolunteerId)
            .NotEmpty()
            .WithError(Errors.General.InvalidValue("volunteerId", "VolunteerId"));

        RuleFor(r => r.Nickname)
            .MustBeValueObject(Nickname.Create);

        RuleFor(r => r.Description)
            .MustBeValueObject(Description.Create);

        RuleFor(r => r.Color)
            .MustBeValueObject(Color.Create);

        RuleFor(r => r.HealthInfo)
            .MustBeValueObject(HealthInfo.Create);

        RuleFor(r => r.Address)
            .MustBeValueObject(a => Address.Create(
                a.District, 
                a.Settlement, 
                a.Street,
                a.House));

        RuleFor(r => r.Weight)
            .NotEmpty()
            .WithError(Errors.General.InvalidValue("weight", "Weight"));

        RuleFor(r => r.Height)
            .NotEmpty()
            .WithError(Errors.General.InvalidValue("height", "Height"));

        RuleFor(r => r.OwnerPhone)
            .MustBeValueObject(PhoneNumber.Create);

        RuleFor(r => r.IsCastrated)
            .NotEmpty()
            .WithError(Errors.General.InvalidValue("isCastrated", "IsCastrated"));

        RuleFor(r => r.DateOfBirth)
            .NotEmpty()
            .WithError(Errors.General.InvalidValue("dateOfBirth", "DateOfBirth"));

        RuleFor(r => r.IsVaccinated)
            .NotEmpty()
            .WithError(Errors.General.InvalidValue("isVaccinated", "IsVaccinated"));

        RuleForEach(r => r.Requisites)
            .MustBeValueObject(r => Requisite.Create(r.Title, r.Description));
    }
}