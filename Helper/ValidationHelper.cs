using System.ComponentModel.DataAnnotations;

namespace HypotheticalRetailStore.Helper;

public class ValidationHelper
{
    public static List<ValidationResult> ValidateModel(object model, out bool isValid)
    {
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();

        isValid = Validator.TryValidateObject(model, context, results, true);

        return results;
    }
}