using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace NewsAppWPF.ValidationClasses
{
    public class EmailValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var email = value as string;
            if (string.IsNullOrWhiteSpace(email))
                return new ValidationResult(false, "Email cannot be empty.");

            var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            return regex.IsMatch(email)
                ? ValidationResult.ValidResult
                : new ValidationResult(false, "Invalid email format.");
        }
    }
}
