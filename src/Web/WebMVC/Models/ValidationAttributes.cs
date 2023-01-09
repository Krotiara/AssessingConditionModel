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


    public class ParamNameSetAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)// Return a boolean value: true == IsValid, false != IsValid
        {
            ParameterNames d = (ParameterNames)value;
            return d != ParameterNames.None;
        }
    }


    public class InfluenceTypeSetAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)// Return a boolean value: true == IsValid, false != IsValid
        {
            InfluenceTypes d = (InfluenceTypes)value;
            return d != InfluenceTypes.None;
        }
    }


    public class InfluenceParamsSetAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)// Return a boolean value: true == IsValid, false != IsValid
        {
            List<PatientParameter> d =  value as List<PatientParameter>;
            return d != null && d.Count > 0;
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
