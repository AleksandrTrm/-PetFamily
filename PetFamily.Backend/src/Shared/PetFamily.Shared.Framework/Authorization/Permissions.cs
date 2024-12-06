namespace PetFamily.Shared.Framework.Authorization;

public static class Permissions
{
    public static class Volunteer
    {
        public const string CREATE_VOLUNTEER = "volunteer.create";
        public const string GET_VOLUNTEERS_WITH_PAGINATION = "volunteer.get.with.pagination";
        public const string GET_VOLUNTEER_BY_ID = "volunteer.get.by.id";
        public const string UPDATE_VOLUNTEER_MAIN_INFO = "volunteer.update.main.info";
        public const string UPDATE_VOLUNTEER_REQUISITES = "volunteer.update.requisites";
        public const string UPDATE_VOLUNTEER_SOCIAL_NETWORKS = "volunteer.update.social.networks";
        public const string DELETE_VOLUNTEER = "volunteer.delete";
    }

    public static class Pet
    {
        public const string GET_PETS_WITH_PAGINATION = "pet.get.with.pagination";
        public const string GET_PET_BY_ID = "pet.get.by.id";
        public const string ADD_PET = "pet.add";
        public const string UPLOAD_PET_FILES = "pet.upload.files";
        public const string DELETE_PET_FILES = "pet.delete.files";
        public const string UPDATE_PET_MAIN_INFO = "pet.update.main.info";
        public const string UPDATE_PET_STATUS = "pet.update.status";
        public const string SET_PET_MAIN_PHOTO = "pet.set.main.photo";
        public const string DELETE_PET = "pet.delete";
    }

    public static class Species
    {
        public const string GET_SPECIES_WITH_PAGINATION = "species.get.with.pagination";
        public const string CREATE_SPECIES = "species.create";
        public const string DELETE_SPECIES = "species.delete";
    }

    public static class Breeds
    {
        public const string GET_BREEDS_BY_SPECIES_ID = "breeds.get.by.species.id";
        public const string CREATE_BREED = "breed.create";
        public const string DELETE_BREED = "breed.delete";
    }
}