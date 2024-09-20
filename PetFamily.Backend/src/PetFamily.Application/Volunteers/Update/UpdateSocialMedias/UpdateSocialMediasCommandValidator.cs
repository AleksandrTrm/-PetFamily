using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;
using PetFamily.Domain.VolunteersManagement.Volunteer.VolunteerValueObjects;

namespace PetFamily.Application.Volunteers.Update.UpdateSocialMedias;

public class UpdateSocialMediasCommandValidator : AbstractValidator<UpdateSocialMediasCommand>
{
    public UpdateSocialMediasCommandValidator()
    {
        RuleFor(r => r.VolunteerId)
            .NotEmpty()
            .WithError(Errors.General.InvalidValue());

        RuleForEach(r => r.SocialMedias)
            .MustBeValueObject(r => SocialMedia.Create(r.Title, r.Link));
    }
}