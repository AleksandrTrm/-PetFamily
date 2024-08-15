namespace PetFamily.Domain.Entities
{
    public class Pet
    {
         public Guid Id { get; }
                
        public string Nickname { get; } = string.Empty;
                
        public string Type { get; } = string.Empty;
        
        public string Description { get; } = string.Empty;
        
        public string Breed { get; } = string.Empty;
        
        public string Color { get; } = string.Empty;
        
        public string HealthInfo { get; } = string.Empty;
        
        public string Address { get; } = string.Empty;
        
        public double Weight { get; }
        
        public double Height { get; }
        
        public string OwnerPhone { get; } = string.Empty;
        
        public bool IsCastrated { get; }
        
        public DateOnly DateOfBirth { get; }
        
        public bool IsVaccinated { get; }
        
        public Status Status { get; }

        public List<Requisite> Requisite { get; } = [];

        public DateTime CreatedAt { get; }
    }
}
