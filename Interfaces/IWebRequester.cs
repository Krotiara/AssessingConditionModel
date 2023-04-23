using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IWebRequester
    {
        Task<HttpResponseMessage> SendRequest(string requestUriStr, string method, string? jsonBody = null);

        Task<T> DeserializeBody<T>(HttpResponseMessage response);
    }
}
