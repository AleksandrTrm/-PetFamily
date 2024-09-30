namespace PetFamily.Application.DTOs.Pets;

public class PetDto
{
    public Guid Id { get; init; }

    public string Nickname { get; init; }

    public Guid VolunteerId { get; init; }
}