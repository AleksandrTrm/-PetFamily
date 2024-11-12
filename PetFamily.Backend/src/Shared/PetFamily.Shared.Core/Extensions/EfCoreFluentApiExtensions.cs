using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PetFamily.Shared.Core.Extensions;

public static class EfCoreFluentApiExtensions
{
    public static PropertyBuilder<IReadOnlyList<TValueObject>> HasValueObjectsJsonConversion<TValueObject, TDto>(
        this PropertyBuilder<IReadOnlyList<TValueObject>> property,
        Func<TValueObject, TDto> toDtoSelector,
        Func<TDto, TValueObject> toValueObjectsSelector)
    {
        return property.HasConversion(
                values => SerializeValueObjectCollection(values, toDtoSelector),
                json => DeserializeDtoCollection(json, toValueObjectsSelector),
                CreateCollectionValueComparer<TValueObject>())
            .HasColumnType("jsonb");
    }

    private static string SerializeValueObjectCollection<TValueObject, TDto>(
        IReadOnlyList<TValueObject> valueObjects, Func<TValueObject, TDto> selector)
    {
        var values = valueObjects.Select(selector);

        return JsonSerializer.Serialize(values, JsonSerializerOptions.Default);
    }

    private static IReadOnlyList<TValueObject> DeserializeDtoCollection<TValueObject, TDto>(
        string json, Func<TDto, TValueObject> selector)

    {
        var values = JsonSerializer.Deserialize<IEnumerable<TDto>>(json) ?? [];

        return values.Select(selector).ToList();
    }

    private static ValueComparer<IReadOnlyList<T>> CreateCollectionValueComparer<T>()
    {
        return new(
            (c1, c2) => c1!.SequenceEqual(c2!),
            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v!.GetHashCode())),
            c => c.ToList());
    }
}