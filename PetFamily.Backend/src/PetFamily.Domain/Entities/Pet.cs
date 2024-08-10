using System.ComponentModel.DataAnnotations;

namespace PetFamily.Domain.Entities
{
    public class Pet
    {
        public Guid Id { get; private set; }

        public string Nickname { get; private set; } = string.Empty;

        public string Type { get; private set; } = string.Empty;

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

        public Status Status { get; private set; }

        public IReadOnlyList<Requisite> Requisite { get; private set; } = [];

        public IReadOnlyList<PetPhoto> PetPhotos { get; private set; } = [];

        public DateTime CreatedAt { get; private set; }
    }
}
