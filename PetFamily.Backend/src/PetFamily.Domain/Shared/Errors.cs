using System.Security.AccessControl;

namespace PetFamily.Domain.Shared;

public static class Errors
{
    public static class General
    {
        public static Error NotFound(Guid? id = null)
        {
            var forId = id == null ? "" : $" for id - {id}";
            return Error.NotFound("record.not.found", $"Record not found{id}");
        }

        public static Error InvalidValue(string? name = null)
        {
            var label = name == null ? "" : $" {name}";
            return Error.Validation("value.is.invalid", $"Value{label} is invalid");
        }

        public static Error InvalidLength(int maxLength, string? name = null)
        {
            var label = name == null ? "" : $" {name}";
            return Error.Validation("invalid.value.length", $"Value{name} has max length - {maxLength}");
        }

        public static Error InvalidCount(int min, string? name = null, int? max = null)
        {
            var label = name == null ? "" : " 'name'";
            var forMaxLabel = max == null ? "" : $" and more than {max}";
            return Error.Validation("out.of.range", $"Value{name} can not be less than {min}{forMaxLabel}");
        }

        public static Error LessThenZero(string? name = null)
        {
            var label = name == null ? "" : $" '{name}'";
            return Error.Validation("value.less.than.zero", $"Value{name} can not be less than zero");
        }
    }
}