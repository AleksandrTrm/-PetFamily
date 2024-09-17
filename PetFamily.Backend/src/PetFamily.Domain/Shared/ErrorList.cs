using System.Collections;

namespace PetFamily.Domain.Shared;

public class ErrorList : IEnumerable<Error>
{
    private List<Error> Errors { get; }

    public ErrorList(IEnumerable<Error> errors) => Errors = [..errors];

    IEnumerator<Error> IEnumerable<Error>.GetEnumerator() =>
        Errors.GetEnumerator();


    public IEnumerator GetEnumerator() =>
        Errors.GetEnumerator();

    public static implicit operator ErrorList(List<Error> errors) =>
        new(errors);

    public static implicit operator ErrorList(Error error) =>
        new([error]);
}