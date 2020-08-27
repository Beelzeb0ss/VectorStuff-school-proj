using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace VectorThing.Utility
{
    public class NumbersOnlyValidationRule : ValidationRule
    {

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            double num;
            try
            {
                if (((string)value).Length > 0)
                    num = Double.Parse((String)value);
            }
            catch (Exception e)
            {
                return new ValidationResult(false, $"Illegal characters or {e.Message}");
            }

            return ValidationResult.ValidResult;
        }

        public NumbersOnlyValidationRule() { }

    }
}
