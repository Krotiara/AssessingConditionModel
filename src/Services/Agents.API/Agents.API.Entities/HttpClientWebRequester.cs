using Interfaces;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text;

namespace Agents.API.Entities
{
    public class HttpWebRequester : IWebRequester
    {
        private readonly ILogger<HttpWebRequester> _logger;

        public HttpWebRequester(ILogger<HttpWebRequester> logger)
        {
            _logger = logger;
        }

        public async Task<HttpResponseMessage?> SendRequest(string requestUriStr, string method, string? jsonBody = null)
        {
            try
            {
                method = method.ToLower();
                var body = jsonBody == null ? null : new StringContent(jsonBody, Encoding.UTF8, "application/json");
                using (HttpClient myClient = new(new HttpClientHandler() { UseDefaultCredentials = true }))
                {
                    if (method == "get")
                        return await myClient.GetAsync(requestUriStr);
                    else if (method == "post")
                        return await myClient.PostAsync(requestUriStr, body);
                    else if (method == "patch")
                        return await myClient.PatchAsync(requestUriStr, body);
                    else if (method == "put")
                        return await myClient.PutAsync(requestUriStr, body);
                    else if (method == "delete")
                        return await myClient.DeleteAsync(requestUriStr);
                    else
                        throw new GetWebResponceException($"Unresolve http method {method}");
                }
            }
            catch(HttpRequestException ex)
            {
                _logger.LogError(ex, $"Request error: request - {requestUriStr}, method - {method}.");
                return null;
            }
        }


        
    }
}
