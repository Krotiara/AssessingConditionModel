using Agents.API.Entities;
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
        private readonly TempModelSettings _tempModelSettings;
        private ConcurrentDictionary<ModelKey, ModelMeta> _metas;

        public PredictionRequestsService(IWebRequester webRequester, 
            IOptions<EnvSettings> settings, 
            IOptions<TempModelSettings> modelSets)
        {
            _metas = new();
            _webRequester = webRequester;
            _modelsServerUrl = settings.Value.ModelsApiUrl;
            _tempModelSettings = modelSets.Value;
        }


        public async Task Init()
        {
            var responce = await _webRequester.SendRequest($"{_modelsServerUrl}/models", "GET");
            if (responce.IsSuccessStatusCode)
            {
                var metas = await responce.DeserializeBody<List<ModelMeta>>();
                foreach (var meta in metas)
                    _metas[new ModelKey() {Id= meta.StorageId, Version= meta.Version }] = meta;
            }
        }


        public async Task<ModelMeta?> Get(ModelKey key)
        {
#warning Если обновилась мета, то здесь не будет обновленной.
            if (_metas.ContainsKey(key))
                return _metas[key];
            var responce = await _webRequester.SendRequest($"{_modelsServerUrl}/models/{key.Id}=={key.Version}", "GET");
            if(responce.IsSuccessStatusCode)
            {
                _metas[key] = await responce.DeserializeBody<ModelMeta>();
                return _metas[key];
            }
            return null; //TODO log
        }


        public async Task<HttpResponseMessage> Predict(ModelKey key, float[] input)
        {
            IPredictRequest request = new PredictRequest() { Id = key.Id, Version = key.Version, Input = input };
            string requestBody = Newtonsoft.Json.JsonConvert.SerializeObject(request);
            string url = $"{_modelsServerUrl}/models/predict";
            var responce = await _webRequester.SendRequest(url, "POST", requestBody);
            return responce;
        }
    }
}
