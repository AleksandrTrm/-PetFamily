using PetFamily.Shared.SharedKernel.ValueObjects.Volunteers.Shared;
using PetFamily.Shared.SharedKernel.ValueObjects.Volunteers.Volunteer;

namespace PetFamily.Infrastructure.Authentication;

public class VolunteerAccount
{
    public int Experience { get; set; }

    public List<Requisite> Requisites { get; set; }

    public List<string> Certificates { get; set; }

    public FullName FullName { get; set; }
    
    public Guid UserId { get; set; }

    public User User { get; set; }
}