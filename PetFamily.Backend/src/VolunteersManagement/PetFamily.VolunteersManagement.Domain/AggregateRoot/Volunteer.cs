using System.Runtime.InteropServices.JavaScript;
using CSharpFunctionalExtensions;
using PetFamily.Shared.Core.Abstractions;
using PetFamily.Shared.SharedKernel;
using PetFamily.Shared.SharedKernel.Error;
using PetFamily.Shared.SharedKernel.IDs;
using PetFamily.Shared.SharedKernel.ValueObjects.Volunteers.Pets;
using PetFamily.Shared.SharedKernel.ValueObjects.Volunteers.Shared;
using PetFamily.Shared.SharedKernel.ValueObjects.Volunteers.Volunteer;
using PetFamily.VolunteersManagement.Domain.Entities.Pets;
using PetFamily.VolunteersManagement.Domain.Entities.Pets.Enums;

namespace PetFamily.VolunteersManagement.Domain.AggregateRoot
{
    public class Volunteer : SoftDeletableEntity<VolunteerId>
    {
        private readonly List<Pet> _pets = []; 
            
        public const int MAX_EXPERIENCE_YEARS = 80;
        public const int MIN_EXPERIENCE_YEARS = 0;
        
        //ef core
        private Volunteer(VolunteerId id) : base(id)
        {
        }

        public Volunteer(
            VolunteerId id, 
            FullName fullName, 
            Description description, 
            int experience,
            PhoneNumber phoneNumber, 
            IReadOnlyList<SocialNetwork> socialMedias, 
            IReadOnlyList<Requisite> requisites) : base(id)
        {
            FullName = fullName;
            Description = description;
            Experience = experience;
            PhoneNumber = phoneNumber;
            SocialMedias = socialMedias;
            Requisites = requisites;
        }

        public FullName FullName { get; private set; }

        public Description Description { get; private set; }

        public int Experience { get; private set; }

        public PhoneNumber PhoneNumber { get; private set; }

        public IReadOnlyList<SocialNetwork> SocialMedias { get; private set; }

        public IReadOnlyList<Requisite> Requisites { get; private set; }

        public IReadOnlyList<Pet> Pets => _pets;

        public int GetCountOfPetsThatFoundHome() =>
            _pets.Count(p => p.Status == Status.FoundHome);
        
        public int GetCountOfPetsThatLookingForHome() =>
            _pets.Count(p => p.Status == Status.LookingForHome);
        
        public int GetCountOfPetsThatGetTreatment() =>
            _pets.Count(p => p.Status == Status.NeedsHelp);

        public override void Delete()
        {
            base.Delete();

            foreach (var pet in _pets) 
                pet.Delete();
        }

        public override void Restore()
        {
            base.Restore();

            foreach (var pet in _pets)
                pet.Restore();
        }

        public void SoftPetDelete(Pet pet)
        {
            var petToDelete = _pets.FirstOrDefault(p => p.Id.Value == pet.Id.Value);
            if (petToDelete is null)
                return;
            
            petToDelete.Delete();
        }

        public int IrreversiblePetsDelete(double deletedEntityLifeTime) =>
            _pets.RemoveAll(p => p is { IsDeleted: true, DeletionTime: not null }
                                 && p.DeletionTime.Value.AddDays(deletedEntityLifeTime) < DateTime.UtcNow);
        
        public void DeleteAllPets() => _pets.Clear();
        
        public void UpdateMainInfo(
            FullName fullName,
            int experience, 
            Description description, 
            PhoneNumber phoneNumber)
        {
            FullName = fullName;
            Experience = experience;
            Description = description;
            PhoneNumber = phoneNumber;
        }

        public void UpdateRequisites(ValueObjectList<Requisite> requisites) => Requisites = requisites;

        public void UpdateSocialMedias(ValueObjectList<SocialNetwork> socialMedias) => SocialMedias = socialMedias;

        public UnitResult<Error> AddPet(Pet pet)
        {
            var serialNumberResult = SerialNumber.Create(_pets.Count + 1);
            if (serialNumberResult.IsFailure)
                return serialNumberResult.Error;

            pet.SetSerialNumber(serialNumberResult.Value);
            
            _pets.Add(pet);
            return Result.Success<Error>();
        }

        public UnitResult<Error> MovePet(PetId petId, SerialNumber serialNumber)
        {
            if (_pets.Count == 0)
                return Error.Failure(
                    "pets.count.was.empty", 
                    "Can not move pet because current volunteer does not have any pets");

            if (serialNumber.Value > _pets.Count)
                return Errors.General.InvalidValue(nameof(serialNumber));

            var pet = _pets.FirstOrDefault(p => p.Id == petId);
            if (pet is null)
                return Errors.General.NotFound(petId.Value);

            if (pet.SerialNumber == serialNumber)
                return Result.Success<Error>();

            var oldNumber = pet.SerialNumber;
            UpdatePositions(serialNumber, oldNumber);
            pet.SetSerialNumber(serialNumber);

            return Result.Success<Error>();
        }

        private void UpdatePositions(SerialNumber newSerialNumber, SerialNumber oldSerialNumber)
        {
            if (newSerialNumber.Value < oldSerialNumber.Value)
            {
                var petsInRange = _pets
                    .Where(p => p.SerialNumber.Value >= newSerialNumber.Value 
                                && p.SerialNumber.Value < oldSerialNumber.Value);
                foreach (var pet in petsInRange)
                {
                    pet.SetSerialNumber(SerialNumber.Create(pet.SerialNumber.Value + 1).Value);
                }
            }
            else if (newSerialNumber.Value > oldSerialNumber.Value)
            {
                var collection = _pets
                    .Where(p => p.SerialNumber.Value > oldSerialNumber.Value 
                                && p.SerialNumber.Value <= newSerialNumber.Value);
                foreach (var entity in collection)
                {
                    entity.SetSerialNumber(SerialNumber.Create(entity.SerialNumber.Value - 1).Value);
                }
            }
        }
        
        public Result<Pet, Error> GetPetById(PetId petId)
        {
            var pet = _pets.FirstOrDefault(p => p.Id.Value == petId.Value);
            if (pet is null)
                return Errors.General.NotFound(petId.Value);

            return pet;
        }
    }
}