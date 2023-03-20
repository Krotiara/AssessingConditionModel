using Agents.API.Entities;
using Agents.API.Entities.Requests;
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
            _delegates[SystemCommands.GetLatestPatientParameters] = async (DateTime startTimestamp, DateTime endTimestamp, int patientId, string medOrganization) =>
            {
                PatientParametersRequest request = new PatientParametersRequest()
                {
                    PatientId = patientId,
                    MedicalOrganization = medOrganization,
                    StartTimestamp = startTimestamp,
                    EndTimestamp = endTimestamp
                };
                string body = Newtonsoft.Json.JsonConvert.SerializeObject(request);
                string url = $"{_patientsResolverApiUrl}/patientsApi/latestPatientParameters";
                IList<PatientParameter> parameters = await _webRequester.GetResponse<IList<PatientParameter>>(url, "POST", body);
                return parameters.ToDictionary(x => x.ParameterName, x=>x);
            };

            _delegates[SystemCommands.GetAge] = async (Dictionary<ParameterNames,PatientParameter> parameters) =>
            {
                if(!parameters.ContainsKey(ParameterNames.Age))
                    throw new NotImplementedException(); //TODO - обработка такого случая.
                return int.Parse(parameters[ParameterNames.Age].Value);
            };

            _delegates[SystemCommands.GetBioageByFunctionalParameters] = async (Dictionary<ParameterNames, PatientParameter> pDict) =>
            {
                float[] inputArgs = new float[] 
                {
                    pDict[ParameterNames.SystolicPressure].ConvertValue<float>(),
                    pDict[ParameterNames.DiastolicPressure].ConvertValue<float>(),
                    pDict[ParameterNames.SystolicPressure].ConvertValue<float>() - pDict[ParameterNames.DiastolicPressure].ConvertValue<float>(),
                    pDict[ParameterNames.InhaleBreathHolding].ConvertValue<float>(),
                    pDict[ParameterNames.OuthaleBreathHolding].ConvertValue<float>(),
                    pDict[ParameterNames.LungCapacity].ConvertValue<float>(),
                    pDict[ParameterNames.Weight].ConvertValue<float>(),
                    pDict[ParameterNames.Accommodation].ConvertValue<float>(),
                    pDict[ParameterNames.HearingAcuity].ConvertValue<float>(),
                    pDict[ParameterNames.StaticBalancing].ConvertValue<float>()
                };
                IPredictRequest request = new PredictRequest() { ModelId = "bioAgeFuncModel", InputArgs = inputArgs };
                string requestBody = Newtonsoft.Json.JsonConvert.SerializeObject(request);
                string url = $"{_modelsApiUrl}/models/predict/";
                float[] res = (await _webRequester.GetResponse<float[]>(url, "POST", requestBody));
                return (int)Math.Ceiling(res.First());
            };

            _delegates[SystemCommands.GetDentistSum] = async (Dictionary<ParameterNames, PatientParameter> pDict) =>
            {
                float[] inputArgs = new float[]
                {
                    pDict[ParameterNames.ReverseSagittalGap].ConvertValue<float>(),
                    pDict[ParameterNames.FirstMolarsNarrowing].ConvertValue<float>(),
                    pDict[ParameterNames.DentistPointsSum].ConvertValue<float>(),
                    pDict[ParameterNames.SagittalSlit].ConvertValue<float>(),
                    pDict[ParameterNames.VerticalDysocclusion].ConvertValue<float>(),
                    pDict[ParameterNames.LessIncisorOverlap].ConvertValue<float>(),
                    pDict[ParameterNames.ContactIncisorOverlapWithoutInjury].ConvertValue<float>(),
                    pDict[ParameterNames.ContactIncisorOverlapWithInjury].ConvertValue<float>(),
                    pDict[ParameterNames.LowerJawBackwardDisplacement].ConvertValue<float>(),
                    pDict[ParameterNames.LowerJawSideDisplacement].ConvertValue<float>(),
                    pDict[ParameterNames.DentitionLengthReductionByTooth].ConvertValue<float>(),
                    pDict[ParameterNames.DentitionLengthReductionByTooths].ConvertValue<float>(),
                    pDict[ParameterNames.Age].ConvertValue<float>(),
                    pDict[ParameterNames.ScoreAfterTreatment].ConvertValue<float>(),
                    pDict[ParameterNames.TreatmentDuration].ConvertValue<float>(),
                    pDict[ParameterNames.TreatmentSteps].ConvertValue<float>(),
                    pDict[ParameterNames.TreatmentApparatuesCount].ConvertValue<float>(),
                };

                IPredictRequest request = new PredictRequest() { ModelId = "rf_children_3_5_treatment", InputArgs = inputArgs };
                float[] res = (await _webRequester.GetResponse<float[]>($"{_modelsApiUrl}/models/predict/", 
                    "POST", Newtonsoft.Json.JsonConvert.SerializeObject(request)));
                return (int)res.First();

            };

            _delegates[SystemCommands.GetAgeRangBy] = async (int age, int bioAge) =>
            {
                int ageDelta = bioAge - age;
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

            _delegates[SystemCommands.GetInfluences] = async (DateTime startTimestamp, DateTime endTimestamp, int patientId, string medOrganization) =>
            {
                PatientInfluencesRequest request = new PatientInfluencesRequest()
                {
                    PatientId = patientId,
                    MedicalOrganization = medOrganization,
                    StartTimestamp = startTimestamp,
                    EndTimestamp = endTimestamp
                };
                string body = Newtonsoft.Json.JsonConvert.SerializeObject(request);
                string url = $"{_patientsResolverApiUrl}/patientsApi/influences";
                return await _webRequester.GetResponse<IList<Influence>>(url, "POST", body);
            };

            _delegates[SystemCommands.GetInfluencesWithoutParameters] = async (DateTime startTimestamp, DateTime endTimestamp, int patientId, string medOrganization) =>
            {
                PatientInfluencesRequest request = new PatientInfluencesRequest()
                {
                    PatientId = patientId,
                    MedicalOrganization = medOrganization,
                    StartTimestamp = startTimestamp,
                    EndTimestamp = endTimestamp
                };
                string body = Newtonsoft.Json.JsonConvert.SerializeObject(request);
                string url = $"{_patientsResolverApiUrl}/patientsApi/influencesWithoutParams";
                return await _webRequester.GetResponse<IList<Influence>>(url, "POST", body);
            };      
        }
    }
}
