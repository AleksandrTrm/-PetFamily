using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Shared.Core.Abstractions;
using PetFamily.Shared.Core.Extensions;
using PetFamily.Shared.SharedKernel;
using PetFamily.Shared.SharedKernel.Error;
using PetFamily.Shared.SharedKernel.ValueObjects.Volunteers.Volunteer;
using PetFamily.VolunteersManagement.Application.Abstractions;
using PetFamily.VolunteersManagement.Application.Commands.Volunteers.Update.UpdateRequisites;

namespace PetFamily.VolunteersManagement.Application.Commands.Volunteers.Update.UpdateSocialMedias;

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

        List<SocialNetwork> socialMedias = [];
        foreach (var socialMedia in command.SocialMedias)
            socialMedias.Add(SocialNetwork.Create(socialMedia.Title, socialMedia.Link).Value);

        var socialMediasToUpdate = new ValueObjectList<SocialNetwork>(socialMedias);
        
        volunteerResult.Value.UpdateSocialMedias(socialMediasToUpdate);

        await _repository.SaveChanges(volunteerResult.Value, cancellationToken);
        
        _logger.LogInformation("Social medias of volunteer with {id} has been updated", command.VolunteerId);

        return command.VolunteerId;
    }
}