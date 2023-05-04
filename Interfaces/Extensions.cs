using Interfaces.Mongo;
using Interfaces.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
            DisplayAttribute displayAttribute = enumValue.GetAttribute<DisplayAttribute>();
            return displayAttribute != null? displayAttribute.Name : "";
        }


        public static T ParseTo<T>(this string inValue)
        {
            TypeConverter converter =
                TypeDescriptor.GetConverter(typeof(T));

            if (!converter.IsValid(inValue))
                throw new ArgumentException();

            return (T)converter.ConvertFromString(null,
                CultureInfo.InvariantCulture, inValue);
        }


        public static bool IsValidToParse(this string inValue, Type parseType)
        {
            TypeConverter converter =
                TypeDescriptor.GetConverter(parseType);
            return converter.IsValid(inValue);
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


        public static void AddMongoService(this IServiceCollection services, IConfiguration conf)
        {
            IConfigurationSection section = conf.GetSection("MongoDBSettings");
            services.Configure<MongoDBSettings>(section);
            services.AddSingleton<MongoService>();
        }


        public static void AddParametersService(this IServiceCollection services, IConfiguration conf)
        {
            services.AddSingleton<ParametersStore>();
            services.AddSingleton<ParametersService>();
        }
    }
}
