using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.Error;

namespace PetFamily.Domain.VolunteersManagement.ValueObjects.Shared
{
    public record Requisite
    {
        private Requisite()
        {
        }
        
        private Requisite(string title, string description)
        {
            Title = title;
            Description = description;
        }

        public string Title { get; }

        public string Description { get; }

        public static Result<Requisite, Error> Create(string title, string description)
        {
            if (string.IsNullOrWhiteSpace(title))
                return Errors.General.InvalidValue(nameof(title));

            if (title.Length > Constants.MAX_LOW_TEXT_LENGTH)
                return Errors.General.InvalidLength(Constants.MAX_LOW_TEXT_LENGTH, nameof(title));

            if (string.IsNullOrWhiteSpace(description))
                return Errors.General.InvalidValue(nameof(description));
            
            if (title.Length > Constants.MAX_HIGH_TEXT_LENGTH)
                return Errors.General.InvalidLength(Constants.MAX_LOW_TEXT_LENGTH, nameof(description));
            
            return new Requisite(title, description);
        }
    }
}