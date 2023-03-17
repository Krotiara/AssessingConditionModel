using Agents.API.Entities;
using Agents.API.Interfaces;
using Interfaces;
using Interfaces.DynamicAgent;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Service.Services
{
    public class CommandActionsProvider : ICommandActionsProvider
    {

        private readonly IMediator _mediator;
        private readonly IWebRequester _webRequester;
        private readonly Dictionary<SystemCommands, Delegate> _delegates;
        private readonly string _patientsResolverApiUrl;
        private readonly string _modelsApiUrl;

        public CommandActionsProvider(IMediator mediator, IWebRequester webRequester)
        {
            this._webRequester = webRequester;
            this._mediator = mediator;
            _patientsResolverApiUrl = Environment.GetEnvironmentVariable("PATIENTRESOLVER_API_URL");
            _modelsApiUrl = Environment.GetEnvironmentVariable("MODELS_API_URL"); //TODO - в отдельный сервис
            _delegates = new Dictionary<SystemCommands, Delegate>();
            InitDelegates();
        }

        public Delegate? GetDelegateByCommandNameWithoutParams(SystemCommands commandName)
        {
            return _delegates.ContainsKey(commandName) ? _delegates[commandName] : null;
        }


        private void InitDelegates()
        {
            
            // TODO Список методов нужно вынести в отдельное место.
            _delegates[SystemCommands.GetLatestPatientParameters] = async (DateTime startTimestamp, DateTime endTimestamp, int patientId) =>
            {
                string body = Newtonsoft.Json.JsonConvert.SerializeObject(new DateTime[2] { startTimestamp, endTimestamp });
                string url = $"{_patientsResolverApiUrl}/patientsApi/latestPatientParameters/{patientId}";
                return await _webRequester
                  .GetResponse<IList<PatientParameter>>(url, "POST", body);
            };

            _delegates[SystemCommands.GetAge] = async (List<PatientParameter> parameters) =>
            {
                IPatientParameter ageParam = parameters.FirstOrDefault(x => x.ParameterName == ParameterNames.Age);
                if (ageParam == null)
                    throw new NotImplementedException(); //TODO - обработка такого случая.
                long age = long.Parse(ageParam.Value);
                return age;
            };

            _delegates[SystemCommands.GetBioage] = async (PredictRequest predictRequest) =>
            {
                try
                {                  
                    string requestBody = Newtonsoft.Json.JsonConvert.SerializeObject(predictRequest);
                    string url = $"{_modelsApiUrl}/models/predict/";
                    return await _webRequester.GetResponse<long>(url, "POST", requestBody);
                }
                catch (GetWebResponceException ex)
                {
                    throw new NotImplementedException();
                }
                catch (Exception unexpectedEx)
                {
                    //TODO
                    throw new NotImplementedException();
                }
            };

            _delegates[SystemCommands.GetAgeRangBy] = async (long age, long bioAge) =>
            {
                long ageDelta = bioAge - age;
                AgentBioAgeStates rang;
                if (ageDelta <= -9)
                    rang = AgentBioAgeStates.RangI;
                else if (ageDelta > -9 && ageDelta <= -3)
                    rang = AgentBioAgeStates.RangII;
                else if (ageDelta > -3 && ageDelta <= 3)
                    rang = AgentBioAgeStates.RangIII;
                else if (ageDelta > 3 && ageDelta <= 9)
                    rang = AgentBioAgeStates.RangIV;
                else
                    rang = AgentBioAgeStates.RangV;
                return rang;
            };

            _delegates[SystemCommands.GetInfluences] = async (DateTime startTimestamp, DateTime endTimestamp, int patientId) =>
            {
                string body = Newtonsoft.Json.JsonConvert.SerializeObject(new DateTime[2] { startTimestamp, endTimestamp });
                string url = $"{_patientsResolverApiUrl}/patientsApi/influences/{patientId}";
                return await _webRequester.GetResponse<IList<Influence>>(url, "POST", body);
            };

            _delegates[SystemCommands.GetInfluencesWithoutParameters] = async (DateTime startTimestamp, DateTime endTimestamp, int patientId) =>
            {
                string body = Newtonsoft.Json.JsonConvert.SerializeObject(new DateTime[2] { startTimestamp, endTimestamp });
                string url = $"{_patientsResolverApiUrl}/patientsApi/influencesWithoutParams/{patientId}";
                return await _webRequester.GetResponse<IList<Influence>>(url, "POST", body);
            };

                //TODO Добавить meta инфу для этого действия
                _delegates[SystemCommands.GetAllInfluences] = async (DateTime startTimestamp, DateTime endTimestamp) =>
            {
                string url = $"{_patientsResolverApiUrl}/patientsApi/influences/";
                string body = Newtonsoft.Json.JsonConvert.SerializeObject(new DateTime[2] { startTimestamp, endTimestamp });
                return await _webRequester.GetResponse<List<Influence>>(url, "POST", body);
            };
        }
    }
}
