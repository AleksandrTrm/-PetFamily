using FluentValidation;
using PetFamily.Domain.ValueObjects;
using PetFamily.Application.Validation;
using PetFamily.Domain.ValueObjects.VolunteerValueObjects;

namespace PetFamily.Application.Volunteers.CreateVolunteer;

public class CreateVolunteerRequestValidator : AbstractValidator<CreateVolunteerRequest>
{
    public CreateVolunteerRequestValidator()
    {
        RuleFor(c => new
            {
                c.FullName.Name,
                c.FullName.Surname,
                c.FullName.Patronymic
            })
            .MustBeValueObject(x => FullName.Create(x.Name, x.Surname, x.Patronymic));
        
        RuleFor(c => c.Description.Description)
            .MustBeValueObject(Description.Create);
        
        RuleFor(c => c.PhoneNumber.PhoneNumber)
            .MustBeValueObject(PhoneNumber.Create);

        RuleForEach(c => c.SocialMedias)
            .MustBeValueObject(s => SocialMedia.Create(s.Title, s.Link));

        RuleForEach(c => c.Requisites)
            .MustBeValueObject(r =>
            {
                var descriptionResult = Description.Create(r.Description.Description);

                if (descriptionResult.IsFailure)
                    return descriptionResult.Error;

                return Requisite.Create(r.Title, descriptionResult.Value);
            });
    }
}