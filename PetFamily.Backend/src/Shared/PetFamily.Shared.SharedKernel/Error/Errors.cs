using System.Text.RegularExpressions;

namespace PetFamily.Shared.SharedKernel.Error;

public static class Errors
{
    public static class General
    {
        public static Error AlreadyExists(string? value = null)
        {
            return Error.Conflict(
                "record.exists",
                $"Record with same parameter '{value}' already exists");
        }

        public static Error NotFound(Guid? id = null, string? title = null)
        {
            var forId = id is null ? "" : $" for id - {id}";
            var withTitle = title is null ? "" : $" '{title}'";
            return Error.NotFound("record.not.found", $"Record{withTitle} not found{forId}");
        }

        public static Error InvalidValue(string? name = null, string? invalidField = null)
        {
            var label = name == null ? "" : $" {name}";
            return Error.Validation("value.is.invalid", $"Value{label} is invalid");
        }

        public static Error InvalidLength(int length, string? name = null, bool? isExceeded = null)
        {
            var label = name == null ? "" : $" {name}";

            return isExceeded == true
                ? Error.Validation("invalid.value.length", $"Value{label} has min length - {length}")
                : Error.Validation("invalid.value.length", $"Value{label} has max length - {length}");
        }

        public static Error InvalidCount(int min, string? name = null, int? max = null)
        {
            var label = name == null ? "" : $" '{name}'";
            var forMaxLabel = max == null ? "" : $" and more than {max}";
            return Error.Validation("out.of.range", $"Value{label} can not be less than {min}{forMaxLabel}");
        }
    }

    public static class Accounts
    {
        public static Error InvalidJwtToken()
        {
            return Error.Validation("token.is.invalid", "Your jwt security token is invalid");
        }

        public static Error ExpiredToken()
        {
            return Error.Validation("token.is.expired", "Your token is expired");
        }

        public static Error InvalidCredentials()
        {
            return Error.Validation(
                "credentials.is.invalid",
                $"Your credentials is invalid");
        }
    }
}