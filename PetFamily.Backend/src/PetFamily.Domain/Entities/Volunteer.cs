namespace PetFamily.Domain.Entities
{
    public class Volunteer
    {
        public Guid Id { get; private set; }

        public string FullName { get; private set; } = string.Empty;

        public string Description { get; private set; } = string.Empty;

        public int Expirience { get; private set; }

        public int CountOfPetsThatFoundHome { get; private set; }

        public int CountOfPetsThatLookingForHome { get; private set; }

        public int CountOfPetsThatGetTreatment { get; private set; }

        public string PhoneNumber { get; private set; } = string.Empty;

        public List<SocialMedia> SocialMedias { get; private set; } = [];

        public List<Requisite> Requisites { get; private set; } = [];

        public List<Pet> Pets { get; private set; } = [];
    }
}
