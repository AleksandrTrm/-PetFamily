using PetFamily.Domain.Shared;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Volunteers.Update.UpdateRequisites;
using PetFamily.Domain.VolunteersManagement.Volunteer.VolunteerValueObjects;

namespace PetFamily.Application.Volunteers.Update.UpdateSocialMedias;

public class UpdateSocialMediasHandler
{
    private readonly IVolunteersRepository _repository;
    private readonly ILogger<UpdateRequisitesHandler> _logger;

    public UpdateSocialMediasHandler(IVolunteersRepository repository, ILogger<UpdateRequisitesHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> Handle(
        UpdateSocialMediasRequest request, 
        CancellationToken cancellationToken = default)
    {
        var volunteerResult = await _repository.GetById(request.VolunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error;

        List<SocialMedia> socialMedias = [];
        foreach (var socialMedia in request.SocialMedias.SocialMedias)
        {
            socialMedias.Add(SocialMedia.Create(socialMedia.Title, socialMedia.Link).Value);
        }

        var socialMediasToUpdate = new SocialMedias(socialMedias);
        
        volunteerResult.Value.UpdateSocialMedias(socialMediasToUpdate);

        var updateResult = await _repository.Save(volunteerResult.Value, cancellationToken);
        
        _logger.LogInformation("Social medias of volunteer with {id} has been updated", request.VolunteerId);

        return request.VolunteerId;
    }
}