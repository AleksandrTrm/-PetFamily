using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.ValueObjects.PetValueObjects;

public record PetPhotos
{
    private const int MIN_PHOTOS_COUNT = 1;
    private const int MAX_PHOTOS_COUNT = 15;
    
    //ef core
    private PetPhotos()
    {
    }
    
    private PetPhotos(List<PetPhoto> value)
    {
        Value = value;
    }
    
    public List<PetPhoto> Value { get; }

    public int PetPhotosCount => Value.Count;
    
    public static Result<PetPhotos, Error> Create(List<PetPhoto> petPhotos)
    {
        if (petPhotos.Count is < MIN_PHOTOS_COUNT or > MAX_PHOTOS_COUNT)
            return Errors.General.InvalidCount(MIN_PHOTOS_COUNT, nameof(PetPhotosCount), MAX_PHOTOS_COUNT);

        return new PetPhotos(petPhotos);
    }
}