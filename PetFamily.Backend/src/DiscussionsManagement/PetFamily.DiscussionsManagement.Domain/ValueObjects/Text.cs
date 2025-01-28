using CSharpFunctionalExtensions;
using PetFamily.Shared.SharedKernel;
using PetFamily.Shared.SharedKernel.Error;

namespace PetFamily.DiscussionsManagement.Domain.ValueObjects;

public class Text : ValueObject
{
    private Text(string content)
    {
        Content = content;
    }
    
    public string Content { get; }

    public static Result<Text, Error> Create(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
            return Errors.General.InvalidValue(nameof(content));

        if (content.Length > Constants.MAX_HIGH_TEXT_LENGTH)
            return Errors.General.InvalidLength(Constants.MAX_HIGH_TEXT_LENGTH, nameof(content), true);

        return new Text(content);
    }
    
    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Content;
    }
}