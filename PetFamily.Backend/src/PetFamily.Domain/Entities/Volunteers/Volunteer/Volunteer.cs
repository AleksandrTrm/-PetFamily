using CSharpFunctionalExtensions;
using PetFamily.Domain.Entities.Volunteers.Pets;
using PetFamily.Domain.ValueObjects;
using PetFamily.Domain.ValueObjects.VolunteerValueObjects;
using Entity = PetFamily.Domain.Shared.Entity<PetFamily.Domain.Entities.Volunteers.Volunteer.VolunteerId>;

namespace PetFamily.Domain.Entities.Volunteers.Volunteer
{
    public class Volunteer : Entity
    {
        public const int MAX_EXPERIENCE_YEARS = 80;

        //ef core
        private Volunteer(VolunteerId id) : base(id)
        {
        }

        public Volunteer(VolunteerId id, FullName fullFullName, Description description, int experience,
            int countOfPetsThatFoundHome, int countOfPetsThatLookingForHome, int countOfPetsThatGetTreatment,
            PhoneNumber phoneNumber, SocialMedias socialMedias, Requisites requisites) : base(id)
        {
            FullFullName = fullFullName;
            Description = description;
            Experience = experience;
            CountOfPetsThatFoundHome = countOfPetsThatFoundHome;
            CountOfPetsThatLookingForHome = countOfPetsThatLookingForHome;
            CountOfPetsThatGetTreatment = countOfPetsThatGetTreatment;
            PhoneNumber = phoneNumber;
            SocialMedias = socialMedias;
            Requisites = requisites;
        }

        public FullName FullFullName { get; private set; }

        public Description Description { get; private set; }

        public int Experience { get; private set; }

        public int CountOfPetsThatFoundHome { get; private set; }

        public int CountOfPetsThatLookingForHome { get; private set; }

        public int CountOfPetsThatGetTreatment { get; private set; }

        public PhoneNumber PhoneNumber { get; private set; }

        public SocialMedias SocialMedias { get; private set; }

        public Requisites Requisites { get; private set; }

        public List<Pet> Pets { get; private set; } = [];

        public static Result<Volunteer, string> Create(VolunteerId id, FullName fullFullName, Description description, int experience,
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

            return new Volunteer(id, fullFullName, description, experience, countOfPetsThatFoundHome,
                countOfPetsThatLookingForHome, countOfPetsThatGetTreatment, phoneNumber, socialMedias, requisites);
        }
    }
}