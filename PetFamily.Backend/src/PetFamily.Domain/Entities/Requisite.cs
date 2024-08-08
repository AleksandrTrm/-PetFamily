namespace PetFamily.Domain.Entities
{
    public class Requisite
    {
        public Guid Id { get; }

        public string Title { get; } = string.Empty;

        public string Description { get; } = string.Empty;
    }
}
