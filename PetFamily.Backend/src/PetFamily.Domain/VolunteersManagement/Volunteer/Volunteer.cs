using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.IDs;
using PetFamily.Domain.ValueObjects;
using PetFamily.Domain.VolunteersManagement.Pets;
using PetFamily.Domain.VolunteersManagement.Pets.Enums;
using PetFamily.Domain.VolunteersManagement.Volunteer.VolunteerValueObjects;

namespace PetFamily.Domain.VolunteersManagement.Volunteer
{
    public class Volunteer : Shared.Entity<VolunteerId>
    {
        private bool _isDeleted = false;
        private readonly List<Pet> _pets; 
            
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

        public void AddPet(Pet pet)
        {
            _pets.Add(pet);
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