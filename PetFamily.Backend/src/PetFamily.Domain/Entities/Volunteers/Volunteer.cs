using PetFamily.Domain.Shared;
using CSharpFunctionalExtensions;
using PetFamily.Domain.ValueObjects;
using PetFamily.Domain.ValueObjects.VolunteerValueObjects;
using Entity = PetFamily.Domain.Shared.Entity;

namespace PetFamily.Domain.Entities.Volunteers
{
    public class Volunteer : Entity
    {
        public const int MAX_EXPERIENCE_YEARS = 80;

        //ef core
        private Volunteer(Guid id) : base(id)
        {
        }

        public Volunteer(Guid id, Name fullName, Description description, int experience,
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

        public Name FullName { get; private set; }

        public Description Description { get; private set; }

        public int Experience { get; private set; }

        public int CountOfPetsThatFoundHome { get; private set; }

        public int CountOfPetsThatLookingForHome { get; private set; }

        public int CountOfPetsThatGetTreatment { get; private set; }

        public PhoneNumber PhoneNumber { get; private set; }

        public SocialMedias SocialMedias { get; private set; }

        public Requisites Requisites { get; private set; }

        public List<Pet> Pets { get; private set; } = [];

        public static Result<Volunteer, string> Create(Guid id, Name fullName, Description description, int experience,
            int countOfPetsThatFoundHome, int countOfPetsThatLookingForHome, int countOfPetsThatGetTreatment,
            PhoneNumber phoneNumber, SocialMedias socialMedias, Requisites requisites)
        {
            if (experience is < 0 or > MAX_EXPERIENCE_YEARS)
                return $"Experience can not be less than zero and more than {MAX_EXPERIENCE_YEARS}";

            if (countOfPetsThatFoundHome is < 0 or > MAX_EXPERIENCE_YEARS)
                return $"Count of pets that are found home can not be less than zero";

            if (countOfPetsThatLookingForHome is < 0 or > MAX_EXPERIENCE_YEARS)
                return $"Count of pets that are looking for home can not be less than zero";

            if (countOfPetsThatGetTreatment is < 0 or > MAX_EXPERIENCE_YEARS)
                return $"Count of pets that are get treatment can not be less than zero";

            return new Volunteer(id, fullName, description, experience, countOfPetsThatFoundHome,
                countOfPetsThatLookingForHome, countOfPetsThatGetTreatment, phoneNumber, socialMedias, requisites);
        }
    }
}