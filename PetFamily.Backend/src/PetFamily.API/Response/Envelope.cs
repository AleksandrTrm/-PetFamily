﻿using PetFamily.Domain.Shared;

namespace PetFamily.API.Response;

public class Envelope
{
    public object? Result { get; }
    
    public string? ErrorCode { get; }
    
    public string? ErrorMessage { get; }
    
    public DateTime TimeGenerated { get; }

    private Envelope(object? result, Error? error)
    {
        Result = result;
        ErrorCode = error?.Code;
        ErrorMessage = error?.Message;
        TimeGenerated = DateTime.Now;
    }

    public static Envelope Ok(object? result) =>
        new Envelope(result, null);

    public static Envelope Error(Error error) =>
        new Envelope(null, error);
}