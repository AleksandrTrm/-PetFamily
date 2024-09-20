namespace PetFamily.Domain.Shared;

public static class Errors
{
    public static class General
    {
        public static Error AlreadyExists(string? value = null)
        {
            var label = value is null ? "" : $" {value}";
            return Error.Conflict(
                "record.exists", 
                $"Record with same parameter '{label}' already exists");
        }
        
        public static Error NotFound(Guid? id = null)
        {
            var forId = id == null ? "" : $" for id - {id}";
            return Error.NotFound("record.not.found", $"Record not found{forId}");
        }

        public static Error InvalidValue(string? name = null, string? invalidField = null)
        {
            var label = name == null ? "" : $" {name}";
            return Error.Validation("value.is.invalid", $"Value{label} is invalid");
        }

        public static Error InvalidLength(int maxLength, string? name = null)
        {
            var label = name == null ? "" : $" {name}";
            return Error.Validation("invalid.value.length", $"Value{label} has max length - {maxLength}");
        }

        public static Error InvalidCount(int min, string? name = null, int? max = null)
        {
            var label = name == null ? "" : $" '{name}'";
            var forMaxLabel = max == null ? "" : $" and more than {max}";
            return Error.Validation("out.of.range", $"Value{label} can not be less than {min}{forMaxLabel}");
        }
    }
}