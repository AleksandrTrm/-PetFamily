using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared.Error;
using PetFamily.Domain.VolunteersManagement.ValueObjects.Volunteer;

namespace PetFamily.Application.Features.Commands.Volunteers.Update.UpdateSocialMedias;

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