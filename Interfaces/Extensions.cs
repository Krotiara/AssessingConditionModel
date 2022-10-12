using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Interfaces
{
    public static class Extensions
    {
        /// <summary>
        ///     A generic extension method that aids in reflecting 
        ///     and retrieving any attribute that is applied to an `Enum`.
        /// </summary>
        public static TAttribute GetAttribute<TAttribute>(this Enum enumValue)
                where TAttribute : Attribute
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<TAttribute>();
        }


        public static string GetDisplayAttributeValue(this Enum enumValue)
        {
            return enumValue.GetAttribute<DisplayAttribute>().Name;
        }


        public static T ParseTo<T>(this string inValue)
        {
            TypeConverter converter =
                TypeDescriptor.GetConverter(typeof(T));

            return (T)converter.ConvertFromString(null,
                CultureInfo.InvariantCulture, inValue);
        }


        public static Dictionary<string, string> GetDisplayAttributes<T>(this T obj)
        {
            Dictionary<string, string> attributes = new Dictionary<string, string>();
            PropertyInfo[] properties = obj.GetType().GetProperties();
            foreach (PropertyInfo property in properties)
            {
                try
                {
                    DisplayNameAttribute attribute = property
                        .GetCustomAttributes(typeof(DisplayNameAttribute), true)
                        .Cast<DisplayNameAttribute>()
                        .Single();
                    attributes[attribute.DisplayName] = property.GetValue(obj).ToString();
                    
                }
                catch (Exception ex)
                {
                    continue;
                }

            }
            return attributes;
        }


        public static T GetValueFromName<T>(this string name) where T : Enum
        {

            name = name.Trim().ToLower();

            var type = typeof(T);

            foreach (var field in type.GetFields())
            {
                if (Attribute.GetCustomAttribute(field, typeof(DisplayAttribute)) is DisplayAttribute attribute)
                {
                    string attributeName = attribute.Name.Trim().ToLower();
                    
                    if (attributeName == name || attributeName.Contains(name))
                    {
                        return (T)field.GetValue(null);
                    }
                }

                if (field.Name == name || field.Name.ToLower() == name)
                {
                    return (T)field.GetValue(null);
                }
            }

            throw new ArgumentOutOfRangeException(nameof(name));
        }


        public static ParameterNames GetParameterByDescription(this string description)
        {
            ParameterNames p = Enum.GetValues(typeof(ParameterNames))
                .Cast<ParameterNames>()
                .FirstOrDefault(x =>
                {
                    try
                    {
                        return x.GetAttribute<ParamDescriptionAttribute>()
                        .DescriptionsSet.Contains(description);
                    }
                    catch(Exception ex)
                    {
                        //TODO более аккуратный отлов ненахождения атрибута
                        return false;
                    }
                });
            
            return p;
        }
    }
}
