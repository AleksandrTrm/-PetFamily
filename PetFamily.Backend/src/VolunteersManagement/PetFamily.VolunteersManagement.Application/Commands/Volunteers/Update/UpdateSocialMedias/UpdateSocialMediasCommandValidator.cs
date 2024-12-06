using FluentValidation;
using PetFamily.Shared.Core.Validation;
using PetFamily.Shared.SharedKernel.Error;
using PetFamily.Shared.SharedKernel.ValueObjects.Volunteers.Volunteer;

namespace PetFamily.VolunteersManagement.Application.Commands.Volunteers.Update.UpdateSocialMedias;

public class UpdateSocialMediasCommandValidator : AbstractValidator<UpdateSocialMediasCommand>
{
    public UpdateSocialMediasCommandValidator()
    {
        RuleFor(r => r.VolunteerId)
            .NotEmpty()
            .WithError(Errors.General.InvalidValue());

        RuleForEach(r => r.SocialMedias)
            .MustBeValueObject(r => SocialNetwork.Create(r.Title, r.Link));
    }
}