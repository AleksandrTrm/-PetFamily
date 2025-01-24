using CSharpFunctionalExtensions;
using PetFamily.Shared.SharedKernel.Error;

namespace PetFamily.DiscussionsManagement.Domain.ValueObjects;

public class UsersPair : ValueObject
{
    private UsersPair(Guid firstUserId, Guid secondUserId)
    {
        FirstUserId = firstUserId;
        SecondUserId = secondUserId;
    }
    
    public Guid FirstUserId { get; }
    public Guid SecondUserId { get; }

    public static Result<UsersPair, Error> Create(Guid firstUserId, Guid secondUserId)
    {
        if (firstUserId == Guid.Empty)
            return Errors.General.InvalidValue("firstUserId");
        
        if (secondUserId == Guid.Empty)
            return Errors.General.InvalidValue("secondUserId");

        if (firstUserId == secondUserId)
            return Error.Conflict("same.id", "The sender and recipient ids can not be equals");

        return new UsersPair(firstUserId, secondUserId);
    }
    
    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return FirstUserId;
        yield return SecondUserId;
    }
}