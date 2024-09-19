using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.IDs;
using PetFamily.Domain.ValueObjects;
using PetFamily.Domain.VolunteersManagement.Pets.Enums;
using PetFamily.Domain.VolunteersManagement.Pets.PetValueObjects;

namespace PetFamily.Domain.VolunteersManagement.Pets
{
    public class Pet : Shared.Entity<PetId>
    {
        private bool _isDeleted = false;
        
        //ef core
        private Pet(PetId id) : base(id)
        {
        }

        public Pet(PetId id, 
            Nickname nickname, 
            SpeciesBreed speciesBreed, 
            Description description,
            Color color, 
            HealthInfo healthInfo, 
            Address address, 
            double weight, 
            double height, 
            PhoneNumber ownerPhone, 
            bool isCastrated, 
            DateTime dateOfBirth, 
            bool isVaccinated, 
            Status status, 
            ValueObjectList<Requisite> requisites, 
            DateTime createdAt, 
            ValueObjectList<PetPhoto> petPhotos) : base(id)
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

        public Color Color { get; private set; }

        public HealthInfo HealthInfo { get; private set; }

        public Address Address { get; private set; }

        public double Weight { get; private set; }

        public double Height { get; private set; }

        public PhoneNumber OwnerPhone { get; private set; }

        public bool IsCastrated { get; private set; }

        public DateTime DateOfBirth { get; private set; }

        public bool IsVaccinated { get; private set; }

        public Status Status { get; private set; }

        public ValueObjectList<Requisite> Requisites { get; private set; }

        public DateTime CreatedAt { get; private set; }

        public ValueObjectList<PetPhoto> PetPhotos { get; private set; }

        public void UpdateFiles(ValueObjectList<PetPhoto> petPhotos)
        {
            PetPhotos = petPhotos;
        }
        
        public void Delete()
        {
            _isDeleted = true;
        }

        public void Recover()
        {
            _isDeleted = false;
        }
    }
}