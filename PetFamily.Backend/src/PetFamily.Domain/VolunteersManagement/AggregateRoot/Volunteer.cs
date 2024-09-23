﻿using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.Error;
using PetFamily.Domain.Shared.IDs;
using PetFamily.Domain.VolunteersManagement.Entities.Pets;
using PetFamily.Domain.VolunteersManagement.Entities.Pets.Enums;
using PetFamily.Domain.VolunteersManagement.ValueObjects.Pet;
using PetFamily.Domain.VolunteersManagement.ValueObjects.Shared;
using PetFamily.Domain.VolunteersManagement.ValueObjects.Volunteer;

namespace PetFamily.Domain.VolunteersManagement.AggregateRoot
{
    public class Volunteer : Shared.Entity<VolunteerId>
    {
        private bool _isDeleted = false;
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
            ValueObjectList<SocialMedia> socialMedias, 
            ValueObjectList<Requisite> requisites) : base(id)
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

        public ValueObjectList<SocialMedia> SocialMedias { get; private set; }

        public ValueObjectList<Requisite> Requisites { get; private set; }

        public IReadOnlyList<Pet> Pets => _pets;

        public int GetCountOfPetsThatFoundHome() =>
            _pets.Count(p => p.Status == Status.FoundHome);
        
        public int GetCountOfPetsThatLookingForHome() =>
            _pets.Count(p => p.Status == Status.LookingForHome);
        
        public int GetCountOfPetsThatGetTreatment() =>
            _pets.Count(p => p.Status == Status.NeedsHelp);

        public void Delete()
        {
            _isDeleted = true;

            foreach (var pet in _pets)
                pet.Delete();
        }

        public void Recover()
        {
            _isDeleted = false;
            
            foreach (var pet in _pets)
                pet.Recover();
        }
        
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

        public void UpdateRequisites(ValueObjectList<Requisite> requisites)
        {
            Requisites = requisites;
        }

        public void UpdateSocialMedias(ValueObjectList<SocialMedia> socialMedias)
        {
            SocialMedias = socialMedias;
        }

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

        public void UpdatePositions(SerialNumber newSerialNumber, SerialNumber oldSerialNumber)
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