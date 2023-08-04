using Agents.API.Entities;
using Agents.API.Entities.Mongo;
using Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Service.Services
{
    public class PredictionRequestsService
    {
        private readonly IWebRequester _webRequester;
        private readonly string _modelsServerUrl;
        private ConcurrentDictionary<string, ModelMeta> _metas;

        public PredictionRequestsService(IWebRequester webRequester, 
            IOptions<EnvSettings> settings)
        {
            _metas = new();
            _webRequester = webRequester;
            _modelsServerUrl = settings.Value.ModelsApiUrl;
        }


        public async Task Init()
        {
            var responce = await _webRequester.SendRequest($"{_modelsServerUrl}/models", "GET");
            if (responce.IsSuccessStatusCode)
            {
                var metas = await responce.DeserializeBody<List<ModelMeta>>();
                foreach (var meta in metas)
                    _metas[meta.Id] = meta;
            }
        }


        public async Task<ModelMeta?> Get(string id)
        {
#warning Если обновилась мета, то здесь не будет обновленной.
            if (_metas.ContainsKey(id))
                return _metas[id];

#warning TODO получение по ID в сервисе моделей машинного обучения.

            var responce = await _webRequester.SendRequest($"{_modelsServerUrl}/models/{id}", "GET");
            if(responce.IsSuccessStatusCode)
            {
                _metas[id] = await responce.DeserializeBody<ModelMeta>();
                return _metas[id];
            }
            return null; //TODO log
        }


#warning TODO правка route point на поулчение id из бд  в сервисе моделей машинного обучения.
        public async Task<HttpResponseMessage> Predict(string id, double[] input)
        {
            IPredictRequest request = new PredictRequest() { Id = id, Input = input };
            string requestBody = Newtonsoft.Json.JsonConvert.SerializeObject(request);
            string url = $"{_modelsServerUrl}/models/predict";
            var responce = await _webRequester.SendRequest(url, "POST", requestBody);
            return responce;
        }
    }
}
