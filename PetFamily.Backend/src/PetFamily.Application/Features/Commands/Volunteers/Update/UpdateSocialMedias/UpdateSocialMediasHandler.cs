using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Extensions;
using PetFamily.Application.Features.Commands.Volunteers.Update.UpdateRequisites;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.Error;
using PetFamily.Domain.VolunteersManagement.ValueObjects.Volunteer;

namespace PetFamily.Application.Features.Commands.Volunteers.Update.UpdateSocialMedias;

public class UpdateSocialMediasHandler : ICommandHandler<Guid, UpdateSocialMediasCommand>
{
    private readonly IVolunteersRepository _repository;
    private readonly ILogger<UpdateRequisitesHandler> _logger;
    private IValidator<UpdateSocialMediasCommand> _validator;

    public UpdateSocialMediasHandler(
        IVolunteersRepository repository, 
        IValidator<UpdateSocialMediasCommand> validator,
        ILogger<UpdateRequisitesHandler> logger)
    {
        _validator = validator;
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        UpdateSocialMediasCommand command, 
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToList();
        
        var volunteerResult = await _repository.GetById(command.VolunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();

        List<SocialMedia> socialMedias = [];
        foreach (var socialMedia in command.SocialMedias)
            socialMedias.Add(SocialMedia.Create(socialMedia.Title, socialMedia.Link).Value);

        var socialMediasToUpdate = new ValueObjectList<SocialMedia>(socialMedias);
        
        volunteerResult.Value.UpdateSocialMedias(socialMediasToUpdate);

        await _repository.SaveChanges(volunteerResult.Value, cancellationToken);
        
        _logger.LogInformation("Social medias of volunteer with {id} has been updated", command.VolunteerId);

        return command.VolunteerId;
    }
}