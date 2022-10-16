using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Entities
{
    public class RestWebRequester : IWebRequester
    {
        public async Task<T> GetResponse<T>(string requestUriStr, string method, string? jsonBody = null)
        {
            try
            {
#warning Вылезает ошибка The remote certificate is invalid according to the validation procedure: RemoteCertificateNameMismatch, RemoteCertificateChainErrors
#warning После замены http на https вылезает c# Cannot determine the frame size or a corrupted frame was received.
                HttpWebRequest webRequest = CreateRequest(requestUriStr, method, jsonBody);
                return await GetResponseAsync<T>(webRequest);
            }
            catch(Exception ex)
            {
                //TODO notmal try catch
                throw ex;
            }
        }


        private HttpWebRequest CreateRequest(string requestUriStr, string method, string? jsonBody = null)
        {
            HttpWebRequest webRequest = WebRequest.Create(requestUriStr) as HttpWebRequest;
            webRequest.Method = method;
            webRequest.Credentials = CredentialCache.DefaultCredentials; //or account you wish to connect as
            webRequest.PreAuthenticate = true;
            webRequest.ContentType = "application/json"; // or xml if it's your preference

            if (jsonBody != null)
            {
                using (Stream webStream = webRequest.GetRequestStream())
                using (StreamWriter requestWriter = new StreamWriter(webStream, System.Text.Encoding.ASCII))
                {
                    requestWriter.Write(jsonBody);
                }
            }

            return webRequest;
        }

        private async Task<T> GetResponseAsync<T>(HttpWebRequest webRequest)
        {
            //TODO try catch
            HttpWebResponse webResponse = (HttpWebResponse)await Task.Factory.FromAsync(
                    webRequest.BeginGetResponse, webRequest.EndGetResponse, null);
            /*webRequest.GetResponse() as HttpWebResponse;*/

            //HttpWebResponse webResponse = webRequest.GetResponse() as HttpWebResponse;

            if (webResponse.StatusCode != HttpStatusCode.Accepted)
                throw new ApplicationException("Unexpected Response Code. - " + webResponse.StatusCode);
            string response;
            using (System.IO.StreamReader readResponse = new System.IO.StreamReader(webResponse.GetResponseStream()))
            {
                response = readResponse.ReadToEnd();
            }

            T res = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(response);
            return res;
        }
    }
}
