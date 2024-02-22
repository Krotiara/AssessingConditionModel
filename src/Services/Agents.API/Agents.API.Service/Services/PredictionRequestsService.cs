using Agents.API.Entities;
using Agents.API.Entities.Documents;
using Agents.API.Entities.Dto;
using Agents.API.Entities.Requests.Responce;
using Interfaces;
using Microsoft.Extensions.Logging;
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
        private readonly ConcurrentDictionary<string, ModelMeta> _metas;
        private readonly ILogger<PredictionRequestsService> _logger;

        public PredictionRequestsService(IWebRequester webRequester,
            ILogger<PredictionRequestsService> logger,
            IOptions<EnvSettings> settings)
        {
            _metas = new();
            _logger = logger;
            _webRequester = webRequester;
            _modelsServerUrl = settings.Value.ModelsApiUrl;
        }


        public async Task<bool> Init()
        {
            var responce = await _webRequester.SendRequest($"{_modelsServerUrl}/modelsApi/all", "GET");
            if (responce != null && responce.IsSuccessStatusCode)
            {
                var metasString = await responce.DeserializeBody<List<string>>(); //Двойная сериализация из-за бага на стороне сервера ML.
                foreach(string json in metasString)
                {
                    ModelMeta meta = await json.DeserializeJson<ModelMeta>();
                    _metas[meta.Id] = meta;
                }
                return true;
            }

            return false;
        }


        public async Task<ModelMeta?> Get(string id)
        {
#warning Если обновилась мета, то здесь не будет обновленной.
            if (_metas.ContainsKey(id))
                return _metas[id];

            var responce = await _webRequester.SendRequest($"{_modelsServerUrl}/modelsApi/{id}", "GET");
            if (responce != null && responce.IsSuccessStatusCode)
            {
                var meta = await responce.DeserializeBody<ModelMeta>();
                if (meta != null)
                {
                    _metas[id] = meta;
                    return _metas[id];
                }                
            }
            _logger.LogError($"Cant get model meta by id = {id}.");
            return null;
        }


        public IEnumerable<ModelMetaDto> GetMetasDto()
        {
            var res = _metas.Values.Select(x=> new ModelMetaDto()
            {
                Id = x.Id,
                Description = x.Description,
                ParamsNames = x.ParamsNames
            });

            return res;
        }


        public async Task<PredictResponce?> Predict(string id, double[] input)
        {

            PredictRequest request = new PredictRequest() { Id = id, Input = input };
            string requestBody = Newtonsoft.Json.JsonConvert.SerializeObject(request);
            string url = $"{_modelsServerUrl}/modelsApi/predict";
            var responce = await _webRequester.SendRequest(url, "POST", requestBody);
            if (responce == null)
            {
                _logger.LogError($"Cant get answer for request {url}");
                return null;
            }
            if (!responce.IsSuccessStatusCode)
            {
                _logger.LogError($"Error in predict: status - {responce.StatusCode}, phase - {responce.ReasonPhrase}.");
                return null;
            }
            return await responce.DeserializeBody<PredictResponce>();
        }
    }
}
