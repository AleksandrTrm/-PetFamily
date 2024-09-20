﻿using FluentValidation;
using PetFamily.Domain.Shared;
using PetFamily.Application.Validation;

namespace PetFamily.Application.Volunteers.Delete;

public class DeleteVolunteerCommandValidator : AbstractValidator<DeleteVolunteerCommand>
{
    public DeleteVolunteerCommandValidator()
    {
        RuleFor(r => r.VolunteerId)
            .NotEmpty()
            .WithError(Errors.General.InvalidValue("volunteerId"));
    }
}