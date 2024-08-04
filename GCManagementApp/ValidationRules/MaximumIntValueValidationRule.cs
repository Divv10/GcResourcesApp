using System.Globalization;
using System.Windows.Controls;

namespace GCManagementApp.ValidationRules
{
    public class MaximumIntValueValidationRule : ValidationRule
    {
        public int MaximumIntValue { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (int.TryParse(value?.ToString(), out int val) && val <= MaximumIntValue)
            {
                return ValidationResult.ValidResult;
            }
            else
            {
                return new ValidationResult(false, string.Format(Properties.Resources.PleaseEnterValueLowerThanX, MaximumIntValue));
            }
        }
    }
}
