using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.IDs;
using PetFamily.Domain.VolunteersManagement.Entities.Pets.Enums;
using PetFamily.Domain.VolunteersManagement.ValueObjects.Pet;
using PetFamily.Domain.VolunteersManagement.ValueObjects.Shared;
using PetFamily.Domain.VolunteersManagement.ValueObjects.Volunteer;

namespace PetFamily.Domain.VolunteersManagement.Entities.Pets
{
    public class Pet : Shared.Entity<PetId>
    {
        private Pet(PetId id) : base(id)
        {
        }

        public Pet(PetId id, 
            Nickname nickname, 
            SpeciesBreed speciesBreed,
            Description description,
            Color color, 
            HealthInfo healthInfo, 
            Address address, 
            double weight, 
            double height, 
            PhoneNumber ownerPhone, 
            bool isCastrated, 
            DateTime dateOfBirth, 
            bool isVaccinated, 
            Status status, 
            DateTime createdAt, 
            IReadOnlyList<Requisite> requisites, 
            IReadOnlyList<PetPhoto> petPhotos) : base(id)
        {
            Nickname = nickname;
            Description = description;
            SpeciesBreed = speciesBreed;
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
            Requisites = requisites;
            CreatedAt = createdAt;
            PetPhotos = petPhotos;
        }

        public Nickname Nickname { get; private set; }

        public SpeciesBreed SpeciesBreed { get; private set; }

        public Description Description { get; private set; }

        public SerialNumber SerialNumber { get; private set; }

        public Color Color { get; private set; }

        public HealthInfo HealthInfo { get; private set; }

        public Address Address { get; private set; }

        public double Weight { get; private set; }

        public double Height { get; private set; }

        public PhoneNumber OwnerPhone { get; private set; }

        public bool IsCastrated { get; private set; }

        public DateTime DateOfBirth { get; private set; }

        public bool IsVaccinated { get; private set; }

        public Status Status { get; private set; }

        public IReadOnlyList<Requisite> Requisites { get; private set; }

        public IReadOnlyList<PetPhoto> PetPhotos { get; private set; }

        public DateTime CreatedAt { get; private set; }

        public bool IsDeleted { get; private set; } = false;

        public void SetSerialNumber(SerialNumber serialNumber) =>
            SerialNumber = serialNumber;
        
        public void UpdateFiles(ValueObjectList<PetPhoto> petPhotos) =>
            PetPhotos = petPhotos;

        public List<PetPhoto> DeletePhotos(IEnumerable<Guid> filesNames)
        {
            var deletedPhotos = new List<PetPhoto>();
            
            var updatedPetPhotos = PetPhotos.ToList();
            foreach (var fileName in filesNames)
            {
                var petPhotoToDelete = PetPhotos.FirstOrDefault(p => p.Path.Contains(fileName.ToString()));
                if (petPhotoToDelete is not null)
                {
                    deletedPhotos.Add(petPhotoToDelete);
                    updatedPetPhotos.Remove(petPhotoToDelete);
                }
            }

            PetPhotos = updatedPetPhotos;

            return deletedPhotos;
        }
        
        public void Delete() =>
            IsDeleted = true;

        public void Recover() =>
            IsDeleted = false;

        public void UpdateStatus(Status status) =>
            Status = status;

        public void SetMainPhoto(PetPhoto petPhoto)
        {
            var photos = PetPhotos.ToList();

            var oldMainPhoto = photos.First(p => p.IsMain);
            photos.Add(PetPhoto.Create(oldMainPhoto.Path, false).Value);
            photos.Remove(oldMainPhoto);

            var oldPhotoToSetMain = photos.First(p => p.Path == petPhoto.Path);
            photos.Remove(oldPhotoToSetMain);
            photos.Insert(0, petPhoto);
            
            PetPhotos = photos;
        }
        
        public void UpdateMainInfo(
            Nickname nickname, 
            SpeciesBreed speciesBreed,
            Description description,
            Color color, 
            HealthInfo healthInfo, 
            Address address, 
            double weight, 
            double height, 
            PhoneNumber ownerPhone, 
            bool isCastrated, 
            DateTime dateOfBirth, 
            bool isVaccinated,
            IReadOnlyList<Requisite> requisites)
        {
            Nickname = nickname;
            SpeciesBreed = speciesBreed;
            Description = description;
            Color = color;
            HealthInfo = healthInfo;
            Address = address;
            Weight = weight;
            Height = height;
            OwnerPhone = ownerPhone;
            IsCastrated = isCastrated;
            IsVaccinated = isVaccinated;
            DateOfBirth = dateOfBirth;
            Requisites = requisites;
        }
    }
}