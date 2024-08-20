using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Entities
{
    public class Volunteer : Entity
    {
        
        //ef core
        private Volunteer(Guid id) : base(id) { }

        public string FullName { get; private set; } = string.Empty;

        public string Description { get; private set; } = string.Empty;

        public int Expirience { get; private set; }

        public int CountOfPetsThatFoundHome { get; private set; }

        public int CountOfPetsThatLookingForHome { get; private set; }

        public int CountOfPetsThatGetTreatment { get; private set; }

        public string PhoneNumber { get; private set; } = string.Empty;

        public SocialMedias SocialMedias { get; private set; }

        public Requisites Requisites { get; private set; }

        public List<Pet> Pets { get; private set; } = [];
    }
}
