using Interfaces.DynamicAgent;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Entities
{
    public static class Extensions
    {
        public static async Task<T> DeserializeBody<T>(this HttpResponseMessage response)
        {
            var body = await response.Content.ReadAsStreamAsync();
            using (StreamReader readResponse = new System.IO.StreamReader(body))
            {
                string res = readResponse.ReadToEnd();
                T desRes = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(res);
                return desRes;
            }
        }


        public static async Task<T> DeserializeJson<T>(this string json)
        {
            T desRes = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
            return desRes;
        }


        public static T ConvertValue<T>(this IProperty property)
        {
#warning Ненадежный каст.
            try
            {
                return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFrom(property.Value);
            }
            catch(Exception ex)
            {
                return (T)property.Value;
            }
        }
    }
}
