using PetFamily.Domain.Shared;
using PetFamily.Domain.ValueObjects;
using PetFamily.Domain.ValueObjects.PetValueObjects;
using Type = PetFamily.Domain.ValueObjects.PetValueObjects.Type;

namespace PetFamily.Domain.Entities
{
    public class Pet
    {
        //ef core
        private Pet(Guid id) { }
        
        public Nickname Nickname { get; private set; }
                
        public Type Type { get; private set; }
        
        public string Description { get; private set; } = string.Empty;
        
        public string Breed { get; private set; } = string.Empty;
        
        public string Color { get; private set; } = string.Empty;
        
        public string HealthInfo { get; private set; } = string.Empty;
        
        public string Address { get; private set; } = string.Empty;
        
        public double Weight { get; private set; }
        
        public double Height { get; private set; }
        
        public string OwnerPhone { get; private set; } = string.Empty;
        
        public bool IsCastrated { get; private set; }
        
        public DateOnly DateOfBirth { get; private set; }
        
        public bool IsVaccinated { get; private set; }
        
        public PetStatus Status { get; private set; }

        public Requisites Requisites { get; private set; }

        public DateTime CreatedAt { get; private set; }
        
        public Volunteer Volunteer { get; private set; }
        
        public PetPhotos PetPhotos { get; private set; }
    }
}
