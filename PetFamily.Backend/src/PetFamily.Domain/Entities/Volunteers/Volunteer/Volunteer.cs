using PetFamily.Domain.Enums;
using PetFamily.Domain.Shared;
using CSharpFunctionalExtensions;
using PetFamily.Domain.ValueObjects;
using PetFamily.Domain.Entities.Volunteers.Pets;
using PetFamily.Domain.ValueObjects.VolunteerValueObjects;
using Entity = PetFamily.Domain.Shared.Entity<PetFamily.Domain.Entities.Volunteers.Volunteer.VolunteerId>;

namespace PetFamily.Domain.Entities.Volunteers.Volunteer
{
    public class Volunteer : Entity
    {
        private readonly List<Pet> _pets; 
            
        public const int MAX_EXPERIENCE_YEARS = 80;
        
        //ef core
        private Volunteer(VolunteerId id) : base(id)
        {
        }

        public Volunteer(
            VolunteerId id, 
            FullName fullFullName, 
            Description description, 
            int experience,
            PhoneNumber phoneNumber, 
            SocialMedias socialMedias, 
            Requisites requisites) : base(id)
        {
            FullFullName = fullFullName;
            Description = description;
            Experience = experience;
            PhoneNumber = phoneNumber;
            SocialMedias = socialMedias;
            Requisites = requisites;
        }

        public FullName FullFullName { get; private set; }

        public Description Description { get; private set; }

        public int Experience { get; private set; }

        public PhoneNumber PhoneNumber { get; private set; }

        public SocialMedias SocialMedias { get; private set; }

        public Requisites Requisites { get; private set; }

        public IReadOnlyList<Pet> Pets => _pets;

        public int GetCountOfPetsThatFoundHome() =>
            _pets.Count(p => p.Status == Status.FoundHome);
        
        public int GetCountOfPetsThatLookingForHome() =>
            _pets.Count(p => p.Status == Status.LookingForHome);
        
        public int GetCountOfPetsThatGetTreatment() =>
            _pets.Count(p => p.Status == Status.NeedsHelp);
        
        public static Result<Volunteer, Error> Create(
            VolunteerId id, 
            FullName fullFullName, 
            Description description, 
            int experience,
            PhoneNumber phoneNumber, 
            SocialMedias socialMedias, 
            Requisites requisites)
        {
            if (experience is < 0 or > MAX_EXPERIENCE_YEARS)
                return Errors.General.InvalidLength(MAX_EXPERIENCE_YEARS, nameof(experience));
            return new Volunteer(id, fullFullName, description, experience, phoneNumber, socialMedias, requisites);
        }
    }
}