﻿using PetFamily.Shared.SharedKernel.Enums;

namespace PetFamily.Shared.SharedKernel.Error;

public record Error
{
    private const string SEPARATOR = "||";
    
    private Error(
        string code, 
        string message,
        ErrorType type, 
        string? invalidField = null)
    {
        Code = code;
        Message = message;
        Type = type;
        InvalidField = invalidField;
    }

    public string Code { get; }
    
    public string Message { get; }
    
    public ErrorType Type { get; }
    
    public string? InvalidField { get; }

    public static Error Validation(string code, string message, string? invalidField = null) =>
        new(code, message, ErrorType.Validation, invalidField);
    
    public static Error NotFound(string code, string message) =>
        new(code, message, ErrorType.NotFound);
    
    public static Error Conflict(string code, string message) =>
        new(code, message, ErrorType.Conflict);
    
    public static Error Failure(string code, string message) =>
        new(code, message, ErrorType.Failure);
    
    public static Error Forbidden(string code, string message) =>
        new(code, message, ErrorType.Forbidden);

    public string Serialize()
    {
        return string.Join(SEPARATOR, Code, Message, Type);
    }

    public static Error Deserialize(string serialized, string? invalidField = null)
    {
        var parts = serialized.Split(SEPARATOR);

        if (parts.Length < 3)
            throw new ArgumentException("Invalid serialized format");
        
        if (Enum.TryParse<ErrorType>(parts[2], out var type) == false)
            throw new ArgumentException("Invalid serialized format");
        
        return new Error(parts[0], parts[1], type, invalidField); 
    }

    public ErrorList ToErrorList() => new([this]);
}