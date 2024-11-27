using PetFamily.Shared.SharedKernel.ValueObjects.Volunteers.Volunteer;

namespace PetFamily.Infrastructure.Authentication;

public class Admin
{
    public Guid UserId { get; set; }

    public User User { get; set; }
    
    public FullName FullName { get; set; }
}