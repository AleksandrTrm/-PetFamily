using PetFamily.Domain.Shared;
using PetFamily.Domain.ValueObjects;
using PetFamily.Domain.ValueObjects.VolunteerValueObjects;

namespace PetFamily.Domain.Entities.Volunteers
{
    public class Volunteer : Entity
    {
        //ef core
        private Volunteer(Guid id) : base(id)
        {
        }

        public Volunteer(Guid id, string fullName, Description description, int experience,
            int countOfPetsThatFoundHome, int countOfPetsThatLookingForHome, int countOfPetsThatGetTreatment,
            PhoneNumber phoneNumber, SocialMedias socialMedias, Requisites requisites) : base(id)
        {
            FullName = fullName;
            Description = description;
            Experience = experience;
            CountOfPetsThatFoundHome = countOfPetsThatFoundHome;
            CountOfPetsThatLookingForHome = countOfPetsThatLookingForHome;
            CountOfPetsThatGetTreatment = countOfPetsThatGetTreatment;
            PhoneNumber = phoneNumber;
            SocialMedias = socialMedias;
            Requisites = requisites;
        }

        public string FullName { get; private set; }

        public Description Description { get; private set; }

        public int Experience { get; private set; }

        public int CountOfPetsThatFoundHome { get; private set; }

        public int CountOfPetsThatLookingForHome { get; private set; }

        public int CountOfPetsThatGetTreatment { get; private set; }

        public PhoneNumber PhoneNumber { get; private set; }

        public SocialMedias SocialMedias { get; private set; }

        public Requisites Requisites { get; private set; }

        public List<Pet> Pets { get; private set; } = [];
    }
}