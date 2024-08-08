using PetFamily.Domain.Validation;
using System.ComponentModel.DataAnnotations;

namespace PetFamily.Domain.Entities
{
    public class Pet
    {
        private Pet(Guid id, string nickname, string type, string description, string breed, string color, 
            string healthInfo, string address, double weight, double height, string ownerPhone, bool isCastrated,
            DateOnly dateOfBirth, bool isVaccinated, Status status, Requisite requisite, DateTime createdAt)
        {
            Id = id;
            Nickname = nickname;
            Type = type;
            Description = description;
            Breed = breed;
            Color = color;
            HealthInfo = healthInfo;
            Address = address;
            Weight = weight;
            Height = height;
            OwnerPhone = ownerPhone;
            IsCastrated = isCastrated;
            DateOfBirth = dateOfBirth;
            IsVaccinated = isVaccinated;
            Status = status;
            CreatedAt = createdAt;
        }

        [Required]
        public Guid Id { get; }

        [Required]
        [StringLength(50)]
        public string Nickname { get; } = string.Empty;

        [Required]        
        public string Type { get; } = string.Empty;

        [Required]
        public string Description { get; } = string.Empty;

        [Required]
        public string Breed { get; } = string.Empty;

        [Required]
        public string Color { get; } = string.Empty;

        [Required]
        public string HealthInfo { get; } = string.Empty;

        [Required]
        public string Address { get; } = string.Empty;

        [Required]
        public double Weight { get; }

        [Required]
        public double Height { get; }

        [Required]
        public string OwnerPhone { get; } = string.Empty;

        [Required]
        public bool IsCastrated { get; }

        [Required]
        public DateOnly DateOfBirth { get; }

        [Required]
        public bool IsVaccinated { get; }

        [Required]
        public Status Status { get; }

        [Required]
        public Requisite Requisite { get; }

        [Required]
        public DateTime CreatedAt { get; }

        public static (Pet Pet, List<string> Erors) Create(Guid id, string nickname, string type, string description, string breed, string color,
            string healthInfo, string address, double weight, double height, string ownerPhone, bool isCastrated,
            DateOnly dateOfBirth, bool isVaccinated, Status status, Requisite requisite, DateTime createdAt)
        {
            Pet pet = new Pet(id, nickname, type, breed, description, color, healthInfo, address, weight, height,
                ownerPhone, isCastrated, dateOfBirth, isVaccinated, status, requisite, createdAt);

            return (pet, ValidateModel.Validate(pet));
        }
    }
}
