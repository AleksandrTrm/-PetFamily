using Microsoft.AspNetCore.Identity;
using PetFamily.Domain.Shared;

namespace PetFamily.API.Response;

public class Envelope
{
    public object? Result { get; }
    
    public List<ResponseError> Errors { get; }
    
    public DateTime TimeGenerated { get; }

    private Envelope(object? result, IEnumerable<ResponseError> errors)
    {
        Result = result;
        Errors = errors.ToList();
        TimeGenerated = DateTime.Now;
    }

    public static Envelope Ok(object? result) =>
        new Envelope(result, []);

    public static Envelope Error(IEnumerable<ResponseError> errors) =>
        new Envelope(null, errors);
}