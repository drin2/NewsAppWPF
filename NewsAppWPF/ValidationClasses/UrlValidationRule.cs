using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace NewsAppWPF.ValidationClasses
{
    public class UrlValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var url = value as string;
            if (string.IsNullOrWhiteSpace(url))
                return new ValidationResult(false, "URL cannot be empty.");

            if (Uri.TryCreate(url, UriKind.Absolute, out var uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps))
                return ValidationResult.ValidResult;

            return new ValidationResult(false, "Invalid URL format.");
        }
    }
}
