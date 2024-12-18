﻿using System.Collections;

namespace PetFamily.Shared.SharedKernel;

public record ValueObjectList<T> : IReadOnlyList<T>
{
    public IReadOnlyList<T> Values { get; }

    public int Count => Values.Count;

    public T this[int index] => Values[index];

    private ValueObjectList()
    {
    }

    public ValueObjectList(IEnumerable<T> values) =>
        Values = new List<T>(values).AsReadOnly();
    
    public IEnumerator<T> GetEnumerator() =>
        Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() =>
        Values.GetEnumerator();

    public static implicit operator ValueObjectList<T>(List<T> values) => new(values);
}