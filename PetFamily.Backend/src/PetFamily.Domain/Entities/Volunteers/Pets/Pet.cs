using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.ValueObjects;
using PetFamily.Domain.ValueObjects.PetValueObjects;

namespace PetFamily.Domain.Entities.Volunteers.Pets
{
    public class Pet : Shared.Entity<PetId>
    {
        //ef core
        private Pet(PetId id) : base(id)
        {
        }

        private Pet(PetId id, 
            Nickname nickname, 
            SpeciesBreed speciesBreed, 
            Description description, 
            string breed,
            string color, 
            string healthInfo, 
            Address address, 
            double weight, 
            double height, 
            PhoneNumber ownerPhone, 
            bool isCastrated, 
            DateOnly dateOfBirth, 
            bool isVaccinated, 
            Status status, 
            Requisites requisites, 
            DateTime createdAt, 
            PetPhotos petPhotos) : base(id)
        {
            Nickname = nickname;
            Description = description;
            SpeciesBreed = speciesBreed;
            Color = color;
            HealthInfo = healthInfo;
            Address = address;
            Weight = weight;
            Height = height;
            OwnerPhone = ownerPhone;
            IsCastrated = isCastrated;
            DateOfBirth = dateOfBirth;
            IsVaccinated = isVaccinated;
            Status = status;
            Requisites = requisites;
            CreatedAt = createdAt;
            PetPhotos = petPhotos;
        }

        public Nickname Nickname { get; private set; }

        public SpeciesBreed SpeciesBreed { get; private set; }

        public Description Description { get; private set; }

        public string Color { get; private set; }

        public string HealthInfo { get; private set; }

        public Address Address { get; private set; }

        public double Weight { get; private set; }

        public double Height { get; private set; }

        public PhoneNumber OwnerPhone { get; private set; }

        public bool IsCastrated { get; private set; }

        public DateOnly DateOfBirth { get; private set; }

        public bool IsVaccinated { get; private set; }

        public Status Status { get; private set; }

        public Requisites Requisites { get; private set; }

        public DateTime CreatedAt { get; private set; }

        public PetPhotos PetPhotos { get; private set; }

        public static Result<Pet, string> Create(PetId id, 
            Nickname nickname, 
            SpeciesBreed speciesBreed,
            Description description,
            string breed, 
            string color, 
            string healthInfo, 
            Address address, 
            double weight, 
            double height,
            PhoneNumber ownerPhone, 
            bool isCastrated, 
            DateOnly dateOfBirth, 
            bool isVaccinated, 
            Status status,
            Requisites requisites, 
            DateTime createdAt, 
            Guid volunteerId, 
            PetPhotos petPhotos)
        {
            if (string.IsNullOrWhiteSpace(breed))
                return "Breed can not be empty";

            if (breed.Length > Constants.MAX_MIDDLE_TEXT_LENGTH)
                return "The count of characters for breed can not" +
                       $" be more than {Constants.MAX_MIDDLE_TEXT_LENGTH}";

            if (string.IsNullOrWhiteSpace(color))
                return "Color can not be empty";

            if (color.Length > Constants.MAX_LOW_TEXT_LENGTH)
                return "The count of characters for color can not" +
                       $" be more than {Constants.MAX_LOW_TEXT_LENGTH}";

            if (string.IsNullOrWhiteSpace(healthInfo))
                return "Health info can not be empty";

            if (healthInfo.Length > Constants.MAX_MIDDLE_HIGH_LENGTH)
                return "The count of characters for health info can not" +
                       $" be more than {Constants.MAX_MIDDLE_HIGH_LENGTH}";

            return new Pet(id, nickname, speciesBreed, description, breed, color, healthInfo, address, weight, height,
                ownerPhone, isCastrated, dateOfBirth, isVaccinated, status, requisites, createdAt, petPhotos);
        }
    }
}