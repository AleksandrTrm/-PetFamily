namespace PetFamily.Shared.SharedKernel.DTOs.Pets;

public class PetDto
{
    public Guid Id { get; init; }

    public string Nickname { get; init; }

    public SpeciesBreedDto SpeciesBreedDto { get; init; }
    
    public string Description { get; init; }
    
    public int SerialNumber { get; set; }
    
    public string Color { get; init; }

    public string HealthInfo { get; init; }

    public AddressDto Address { get; init; }

    public double Weight { get; init; }

    public double Height { get; init; }

    public string OwnerPhone { get; init; }

    public bool IsCastrated { get; init; }

    public DateTime DateOfBirth { get; init; }

    public bool IsVaccinated { get; init; }

    public string Status { get; init; }

    public IEnumerable<RequisiteDto> Requisites { get; init; }

    public IEnumerable<PetPhotoDto> PetPhotos { get; init; }

    public DateTime CreatedAt { get; init; }
    
    public Guid VolunteerId { get; init; }
}