using CSharpFunctionalExtensions;
using PetFamily.Shared.SharedKernel.Error;
using PetFamily.VolunteerRequestManagement.Domain.Enums;
using PetFamily.VolunteerRequestManagement.Domain.Models;

namespace PetFamily.VolunteerRequestManagement.Domain;

public class VolunteerRequest
{
    public Guid VolunteerRequestId { get; private set; }

    public Guid UserId { get; private set; }
    
    public Guid AdminId { get; private set; }
    
    public Guid DiscussionId { get; private set; }

    public string RejectionComment { get; private set; }

    public RequestStatuses RequestStatus { get; private set; }

    public VolunteerInfo VolunteerInfo { get; private set; } = default!;

    public DateTime CreatedAt { get; private set; }

    public static VolunteerRequest CreateRequest(
        Guid volunteerRequestId,
        Guid userId,
        Guid discussionId,
        VolunteerInfo volunteerInfo)
    {
        return new VolunteerRequest
        {
            VolunteerRequestId = volunteerRequestId, 
            UserId = userId,
            DiscussionId = discussionId,
            RequestStatus = RequestStatuses.Submitted,
            VolunteerInfo = volunteerInfo,
            CreatedAt = DateTime.UtcNow 
        };
    }

    public void AssignRequest(Guid adminId)
    {
        AdminId = adminId;
        RequestStatus = RequestStatuses.OnReview;
    }

    public UnitResult<Error?> SendRequestForRevision(string rejectionComment)
    {
        if (AdminId == Guid.Empty)
            return Errors.General.InvalidValue(nameof(AdminId));
        
        RejectionComment = rejectionComment;
        RequestStatus = RequestStatuses.RevisionRequired;
        
        return UnitResult.Success<Error?>();
    }

    public void RejectRequest() => RequestStatus = RequestStatuses.Rejected;

    public void ApproveRequest() => RequestStatus = RequestStatuses.Approved;
}