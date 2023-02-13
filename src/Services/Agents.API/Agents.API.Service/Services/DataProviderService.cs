using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Service.Services
{
    public class DataProviderService : IDataProviderService
    {
        private readonly IWebRequester webRequester;
        private readonly string codeExecutorUrl; //TODO вынести получение во внешний сервис

        public DataProviderService(IWebRequester webRequester)
        {
            this.webRequester = webRequester;
            codeExecutorUrl = Environment.GetEnvironmentVariable("CODE_EXECUTOR_API_URL");
        }

        public async Task<T> ExecuteSystemCommand<T>(SystemCommands command, object[] args)
        {
            string url = $"{codeExecutorUrl}/codeExecutor/executeCommand/{command}";
            string body = Newtonsoft.Json.JsonConvert.SerializeObject(args);
            T res = await webRequester.GetResponse<T>(url, "POST", body);
            return res;
        }
    }
}
