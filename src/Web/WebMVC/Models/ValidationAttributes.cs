using Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace WebMVC.Models
{
    public class GenderSetAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)// Return a boolean value: true == IsValid, false != IsValid
        {
            GenderEnum d = (GenderEnum)value;
            return d != GenderEnum.None;

        }
    }


    public class DateSetAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)// Return a boolean value: true == IsValid, false != IsValid
        {
#warning не работает.
            DateTime dateTime;
            var isValid = DateTime.TryParseExact(Convert.ToString(value),
            "0:yyyy-MM-dd",
            CultureInfo.CurrentCulture,
            DateTimeStyles.None,
            out dateTime);

            if (isValid)
                isValid = dateTime != default;
            return isValid;

        }
    }
}
