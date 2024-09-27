using System.Text.Json;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PetFamily.Infrastructure.Extensions;

public static class EfCoreFluentApiExtensions
{
    public static PropertyBuilder<IReadOnlyList<TValueObject>> HasValueObjectsJsonConversion<TValueObject, TDto>(
        this PropertyBuilder<IReadOnlyList<TValueObject>> property,
        Func<TValueObject, TDto> toDtoSelector,
        Func<TDto, TValueObject> toValueObjectsSelector)
    {
        return property.HasConversion(
            values => JsonSerializer.Serialize(values, JsonSerializerOptions.Default),
            json => );
    }

    private static string SerializeValueObjectCollection<TValueObject, TDto>(
        IReadOnlyList<TValueObject> valueObjects, Func<TValueObject, TDto> selector)
    {
        var values = valueObjects.Select(selector);

        return JsonSerializer.Serialize(values, JsonSerializerOptions.Default);
    }

    private static IReadOnlyList<TValueObject> DeserializeDtoCollection<TValueObject, TDto>(
        string json, Func<TValueObject, TDto> selector)

    {
        var values = JsonSerializer.Deserialize<IEnumerable<>>(json);
        
        return 
    }
}