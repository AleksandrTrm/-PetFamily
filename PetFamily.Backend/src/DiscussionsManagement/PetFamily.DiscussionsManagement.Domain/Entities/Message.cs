using PetFamily.DiscussionsManagement.Domain.ValueObjects;

namespace PetFamily.DiscussionsManagement.Domain.Entities;

public class Message
{
    public Message(Guid id, Text text, Guid senderId)
    {
        Id = id;
        Text = text;
        SenderId = senderId;
        CreatedAt = DateTime.UtcNow;
    }
    
    public Guid Id { get; private set; }

    public Text Text { get; private set; }

    public Guid SenderId { get; private set; }

    public bool IsEdited { get; private set; }
    
    public DateTime CreatedAt { get; private set; }

    internal void Edit(Text text)
    {
        Text = text;

        IsEdited = true;
    } 
}