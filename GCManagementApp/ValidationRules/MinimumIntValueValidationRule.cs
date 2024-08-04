using System.Globalization;
using System.Windows.Controls;

namespace GCManagementApp.ValidationRules
{
    public class MinimumIntValueValidationRule : ValidationRule
    {
        public int MinimumIntValue { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (int.TryParse(value?.ToString(), out int val) && val >= MinimumIntValue)
            {
                return ValidationResult.ValidResult;
            }
            else
            {
                return new ValidationResult(false, string.Format(Properties.Resources.PleaseEnterValueHigherThanX, MinimumIntValue));
            }
        }
    }
}
