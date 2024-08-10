namespace PetFamily.Domain.Entities
{
    public class Volunteer
    {
        public Guid Id { get; private set; }

        public string FullName { get; private set; } = default!;

        public string Description { get; private set; } = default!;

        public int Expirience { get; private set; }

        public int CountOfPetsThatFoundHome { get; private set; }

        public int CountOfPetsThatLookingForHome { get; private set; }

        public int CountOfPetsThatGetTreatment { get; private set; }

        public string PhoneNumber { get; private set; } = default!;

        public IReadOnlyList<SocialMedia> SocialMedias { get; private set; } = [];

        public IReadOnlyList<Requisite> Requisites { get; private set; } = [];

        public IReadOnlyList<Pet> Pets { get; private set; } = [];
    }
}
