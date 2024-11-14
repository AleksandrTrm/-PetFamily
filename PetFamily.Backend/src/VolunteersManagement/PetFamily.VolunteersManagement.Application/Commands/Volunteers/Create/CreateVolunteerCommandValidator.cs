using FluentValidation;
using PetFamily.Shared.Core.Validation;
using PetFamily.Shared.SharedKernel.Error;
using PetFamily.VolunteersManagement.Domain.AggregateRoot;
using PetFamily.Shared.SharedKernel.ValueObjects.Volunteers.Shared;
using PetFamily.Shared.SharedKernel.ValueObjects.Volunteers.Volunteer;

namespace PetFamily.VolunteersManagement.Application.Commands.Volunteers.Create;

public class CreateVolunteerCommandValidator : AbstractValidator<CreateVolunteerCommand>
{
    public CreateVolunteerCommandValidator()
    {
        RuleFor(c => c.FullName)
            .MustBeValueObject(x => FullName.Create(x.Name, x.Surname, x.Patronymic));
        
        RuleFor(c => c.Description)
            .MustBeValueObject(Description.Create);
        
        RuleFor(c => c.PhoneNumber)
            .MustBeValueObject(PhoneNumber.Create);

        RuleForEach(c => c.SocialMedias)
            .MustBeValueObject(s => SocialMedia.Create(s.Title, s.Link));

        RuleFor(e => e.Experience)
            .Must(e => e is >= Volunteer.MIN_EXPERIENCE_YEARS and < Volunteer.MAX_EXPERIENCE_YEARS)
            .WithError(Errors.General.InvalidCount(
                Volunteer.MIN_EXPERIENCE_YEARS, "experience", Volunteer.MAX_EXPERIENCE_YEARS));
        
        RuleForEach(c => c.Requisites)
            .MustBeValueObject(r => Requisite.Create(r.Title, r.Description));
    }
}