using Microsoft.AspNetCore.Identity;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.Error;

namespace PetFamily.API.Response;

public class Envelope
{
    public object? Result { get; }
    
    public ErrorList? Errors { get; }
    
    public DateTime TimeGenerated { get; }

    private Envelope(object? result, ErrorList? errors)
    {
        Result = result;
        Errors = errors;
        TimeGenerated = DateTime.Now;
    }

    public static Envelope Ok(object? result = null) =>
        new Envelope(result, null);

    public static Envelope Error(ErrorList errors) =>
        new Envelope(null, errors);
}