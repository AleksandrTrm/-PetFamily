using System.ComponentModel.DataAnnotations;

namespace PetFamily.Domain.Validation
{
    public class ValidateModel
    {
        public static List<string> Validate(object obj)
        {
            List<string> errors = new List<string>();

            var context = new ValidationContext(obj);
            var results = new List<ValidationResult>();

            if (!Validator.TryValidateObject(obj, context, results, true))
            {
                foreach (var error in results)
                    if (error.ErrorMessage != null)
                        errors.Add(error.ErrorMessage);
            }

            return errors;
        }
    }
}
