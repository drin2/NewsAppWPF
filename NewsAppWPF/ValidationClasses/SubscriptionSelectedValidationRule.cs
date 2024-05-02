using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace NewsAppWPF.ValidationClasses
{
    public class SubscriptionSelectedValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null)
                return new ValidationResult(false, "Please select a subscription type.");
            return ValidationResult.ValidResult;
        }
    }
}
