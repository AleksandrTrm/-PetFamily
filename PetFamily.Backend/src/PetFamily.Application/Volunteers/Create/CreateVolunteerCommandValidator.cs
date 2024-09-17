using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.ValueObjects;
using PetFamily.Domain.VolunteersManagement.Volunteer.VolunteerValueObjects;

namespace PetFamily.Application.Volunteers.Create;

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

        RuleForEach(c => c.Requisites)
            .MustBeValueObject(r =>
            {
                var descriptionResult = Description.Create(r.Description);

                if (descriptionResult.IsFailure)
                    return descriptionResult.Error;

                return Requisite.Create(r.Title, descriptionResult.Value);
            });
    }
}