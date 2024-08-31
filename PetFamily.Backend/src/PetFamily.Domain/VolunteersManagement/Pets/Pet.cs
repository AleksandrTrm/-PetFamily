using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.Error;
using PetFamily.Domain.Shared.IDs;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.VolunteersManagement.Pets.Enums;
using PetFamily.Domain.VolunteersManagement.Pets.PetValueObjects;

namespace PetFamily.Domain.VolunteersManagement.Pets
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

        public static Result<Pet, Error> Create(PetId id, 
            Nickname nickname, 
            SpeciesBreed speciesBreed,
            Description description,
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
            if (string.IsNullOrWhiteSpace(color))
                return Errors.General.InvalidValue(nameof(color));

            if (color.Length > Constants.MAX_LOW_TEXT_LENGTH)
                return Errors.General.InvalidLength(Constants.MAX_LOW_TEXT_LENGTH, nameof(color));

            if (string.IsNullOrWhiteSpace(healthInfo))
                return Errors.General.InvalidValue(nameof(color));

            if (healthInfo.Length > Constants.MAX_MIDDLE_HIGH_LENGTH)
                return Errors.General.InvalidLength(Constants.MAX_MIDDLE_HIGH_LENGTH, nameof(healthInfo));

            return new Pet(id, nickname, speciesBreed, description, color, healthInfo, address, weight, height,
                ownerPhone, isCastrated, dateOfBirth, isVaccinated, status, requisites, createdAt, petPhotos);
        }
    }
}