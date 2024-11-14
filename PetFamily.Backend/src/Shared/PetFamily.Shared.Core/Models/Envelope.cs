using PetFamily.Shared.SharedKernel.Error;

namespace PetFamily.Shared.Core.Models;

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