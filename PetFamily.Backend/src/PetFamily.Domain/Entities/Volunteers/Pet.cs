﻿using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.ValueObjects;
using PetFamily.Domain.ValueObjects.PetValueObjects;
using Entity = PetFamily.Domain.Shared.Entity;

namespace PetFamily.Domain.Entities.Volunteers
{
    public class Pet : Entity
    {
        //ef core
        private Pet(Guid id) : base(id)
        {
        }

        private Pet(Guid id, Nickname nickname, PetType type, Description description, string breed, string color,
            string healthInfo, Address address, double weight, double height, PhoneNumber ownerPhone, bool isCastrated,
            DateOnly dateOfBirth, bool isVaccinated, Status status, Requisites requisites, DateTime createdAt,
            Volunteer volunteer, PetPhotos petPhotos) : base(id)
        {
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
            Requisites = requisites;
            CreatedAt = createdAt;
            Volunteer = volunteer;
            PetPhotos = petPhotos;
        }

        public Nickname Nickname { get; private set; }

        public PetType Type { get; private set; }

        public Description Description { get; private set; }

        public string Breed { get; private set; }

        public string Color { get; private set; }

        public string HealthInfo { get; private set; }

        public Address Address { get; private set; }

        public double Weight { get; private set; }

        public double Height { get; private set; }

        public PhoneNumber OwnerPhone { get; private set; }

        public bool IsCastrated { get; private set; }

        public DateOnly DateOfBirth { get; private set; }

        public bool IsVaccinated { get; private set; }

        public Status Status { get; private set; }

        public Requisites Requisites { get; private set; }

        public DateTime CreatedAt { get; private set; }

        public Volunteer Volunteer { get; private set; }

        public PetPhotos PetPhotos { get; private set; }

        public static Result<Pet, string> Create(Guid id, Nickname nickname, PetType type, Description description,
            string breed, string color, string healthInfo, Address address, double weight, double height,
            PhoneNumber ownerPhone, bool isCastrated, DateOnly dateOfBirth, bool isVaccinated, Status status,
            Requisites requisites, DateTime createdAt, Guid volunteerId, Volunteer volunteer, PetPhotos petPhotos)
        {
            if (string.IsNullOrWhiteSpace(breed))
                return "Breed can not be empty";

            if (breed.Length > Constants.MAX_MIDDLE_TEXT_LENGTH)
                return "The count of characters for breed can not" +
                       $" be more than {Constants.MAX_MIDDLE_TEXT_LENGTH}";

            if (string.IsNullOrWhiteSpace(color))
                return "Color can not be empty";

            if (color.Length > Constants.MAX_LOW_TEXT_LENGTH)
                return "The count of characters for color can not" +
                       $" be more than {Constants.MAX_LOW_TEXT_LENGTH}";

            if (string.IsNullOrWhiteSpace(healthInfo))
                return "Health info can not be empty";

            if (healthInfo.Length > Constants.MAX_MIDDLE_HIGH_LENGTH)
                return "The count of characters for health info can not" +
                       $" be more than {Constants.MAX_MIDDLE_HIGH_LENGTH}";

            return new Pet(id, nickname, type, description, breed, color, healthInfo, address, weight, height,
                ownerPhone, isCastrated, dateOfBirth, isVaccinated, status, requisites, createdAt, volunteer, 
                petPhotos);
        }
    }
}