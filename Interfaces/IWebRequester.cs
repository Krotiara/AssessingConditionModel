using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IWebRequester
    {
        public Task<T> GetResponse<T>(string requestUriStr, string method, string? jsonBody = null);

        public Task SendRequest(string requestUriStr, string method, string? jsonBody = null);

       
    }
}
