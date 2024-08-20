using CSharpFunctionalExtensions;

namespace PetFamily.Domain.ValueObjects.PetValueObjects;

public record PetPhotos
{
    private PetPhotos(List<PetPhoto> petPhotos)
    {
        Value = petPhotos;
    }
    
    public List<PetPhoto> Value { get; }

    public static Result<PetPhotos, string> Create(List<PetPhoto> petPhotos)
    {
        if (petPhotos.Count is < 1 or > 15)
            return "Count of pet photos can not be less than one and more than fifteen";

        return new PetPhotos(petPhotos);
    }
}