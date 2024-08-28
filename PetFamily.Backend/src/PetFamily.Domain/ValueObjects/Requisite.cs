using PetFamily.Domain.Shared;
using CSharpFunctionalExtensions;

namespace PetFamily.Domain.ValueObjects
{
    public record Requisite
    {
        private Requisite()
        {
        }
        
        private Requisite(string title, Description description)
        {
            Title = title;
            Description = description;
        }

        public string Title { get; }

        public Description Description { get; }

        public static Result<Requisite, Error> Create(string title, Description description)
        {
            if (string.IsNullOrWhiteSpace(title))
                return Errors.General.InvalidValue(nameof(title));

            if (title.Length > Constants.MAX_LOW_TEXT_LENGTH)
                return Errors.General.InvalidLength(Constants.MAX_LOW_TEXT_LENGTH, nameof(title));

            return new Requisite(title, description);
        }
    }
}