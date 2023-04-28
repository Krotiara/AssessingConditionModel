using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Service
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
    }
}
