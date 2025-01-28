using CSharpFunctionalExtensions;
using PetFamily.DiscussionsManagement.Domain.Entities;
using PetFamily.DiscussionsManagement.Domain.ValueObjects;
using PetFamily.Shared.SharedKernel.Error;

namespace PetFamily.DiscussionsManagement.Domain.AggregateRoot;

public class Discussion
{
    private List<Message> _messages = [];

    public Discussion(Guid id, UsersPair usersPair)
    {
        Id = id;
        UsersPair = usersPair;
        IsOpened = true;
    }

    public Guid Id { get; private set; }

    public UsersPair UsersPair { get; }

    public bool IsOpened { get; private set; }

    public IReadOnlyList<Message> Messages => _messages;

    public UnitResult<Error> Comment(Message message)
    {
        if (UsersPair.FirstUserId != message.SenderId && UsersPair.SecondUserId != message.SenderId)
            return Error.Forbidden("not.discussion.member", "You are not a discussion member");

        _messages.Add(message);

        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> Delete(Guid userId, Message message)
    {
        if (!_messages.Contains(message))
            return Errors.General.NotFound(message.Id, nameof(message));
        
        if (userId != message.SenderId)
            return Error.Forbidden(
                "message.not.allowed", 
                "You can not delete message that does not belong you");

        _messages.Remove(message);

        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> EditMessage(Guid userId, Message message, Text content)
    {
        if (userId != message.SenderId)
            return Error.Forbidden(
                "message.not.allowed", 
                "You can not edit message that does not belong you");
     
        message.Edit(content);

        return UnitResult.Success<Error>();
    }

    public void CloseDiscussion() => IsOpened = false;
}