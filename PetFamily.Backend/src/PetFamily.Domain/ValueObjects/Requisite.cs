using PetFamily.Domain.Shared;
using CSharpFunctionalExtensions;

namespace PetFamily.Domain.ValueObjects
{
    public record Requisite
    {
        private Requisite(string title, Description description)
        {
            Title = title;
            Description = description;
        }

        public string Title { get; }

        public Description Description { get; }

        public static Result<Requisite, string> Create(string title, Description description)
        {
            if (string.IsNullOrWhiteSpace(title))
                return "Title can not be empty";

            if (title.Length > Constants.MAX_LOW_TEXT_LENGTH)
                return $"The count of characters for title can not be more than {Constants.MAX_LOW_TEXT_LENGTH}";

            return new Requisite(title, description);
        }
    }
}