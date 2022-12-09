using Interfaces;
using System.Net;
using System.Text;

namespace WebMVC.Models
{
    public class HttpClientWebRequester : IWebRequester
    {
        public async Task<T> GetResponse<T>(string requestUriStr, string method, string? jsonBody = null)
        {
            try
            {
                Stream responce = await GetResponce(requestUriStr, method, jsonBody);
                using (System.IO.StreamReader readResponse = new System.IO.StreamReader(responce))
                {
                    string res = readResponse.ReadToEnd();
                    T desRes = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(res);
                    return desRes;
                }
            }
            catch (ApplicationException ex)
            {
                throw new GetWebResponceException("unexpected response code", ex);
            }
            catch (Exception ex)
            {
                throw new GetWebResponceException("get responce error", ex);
            }
        }


        private async Task<Stream> GetResponce(string requestUriStr, string method, string? jsonBody = null)
        {
            method = method.ToLower();
            using (HttpClient myClient = new HttpClient(new HttpClientHandler() { UseDefaultCredentials = true }))
            {
                if (method == "get")
                {
                    HttpResponseMessage response = await myClient.GetAsync(requestUriStr);
                    return await response.Content.ReadAsStreamAsync();
                }
                else if (method == "post")
                {
                    StringContent data = new StringContent(jsonBody, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await myClient.PostAsync(requestUriStr, data);
                    return await response.Content.ReadAsStreamAsync();
                }
                else
                    throw new Exception($"Unresolve http method {method}");
            }
        }
    }
}
