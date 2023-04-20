using Agents.API.Entities;
using Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Service.AgentCommand
{
    public class GetDentistSumCommand : IAgentCommand
    {

        private readonly IWebRequester _webRequester;
        private readonly string _modelsServerUrl;
        private readonly EnvSettings _settings;

        public GetDentistSumCommand(IWebRequester webRequester, IOptions<EnvSettings> settings)
        {
            _webRequester = webRequester;
            _modelsServerUrl = settings.Value.ModelsApiUrl;
            _settings = settings.Value;
        }

        public Delegate Command => async (Dictionary<ParameterNames, PatientParameter> pDict) =>
        {
            float age = pDict[ParameterNames.Age].ConvertValue<float>();
#warning Костыльное получение версии и Id.
            ModelKey model = GetModelByAge(age);
            if (model == null)
                throw new NotImplementedException(); //TODo
            float[] inputArgs = null;
            //Костыли.
            if (age <= 5)
            {
                inputArgs = new float[]
                {
                    pDict[ParameterNames.ReverseSagittalGap].ConvertValue<float>(),
                    pDict[ParameterNames.FirstMolarsNarrowing].ConvertValue<float>(),
                    pDict[ParameterNames.DentistPointsSum].ConvertValue<float>(),
                    pDict[ParameterNames.SagittalSlit].ConvertValue<float>(),
                    pDict[ParameterNames.VerticalDysocclusion].ConvertValue<float>(),
                    pDict[ParameterNames.LessIncisorOverlap].ConvertValue<float>(),
                    pDict[ParameterNames.ContactIncisorOverlapWithoutInjury].ConvertValue<float>(),
                    pDict[ParameterNames.ContactIncisorOverlapWithInjury].ConvertValue<float>(),
                    pDict[ParameterNames.LowerJawForwardDisplacement].ConvertValue<float>(),
                    pDict[ParameterNames.LowerJawBackwardDisplacement].ConvertValue<float>(),
                    pDict[ParameterNames.LowerJawSideDisplacement].ConvertValue<float>(),
                    pDict[ParameterNames.DentitionLengthReductionByTooth].ConvertValue<float>(),
                    pDict[ParameterNames.DentitionLengthReductionByTooths].ConvertValue<float>(),
                    age,
                    pDict[ParameterNames.TreatmentDuration].ConvertValue<float>(),
                    pDict[ParameterNames.TreatmentSteps].ConvertValue<float>(),
                    pDict[ParameterNames.TreatmentApparatuesCount].ConvertValue<float>()
                };
            }
            else
            {
                inputArgs = new float[]
                {
                    pDict[ParameterNames.ReverseSagittalGap].ConvertValue<float>(),
                    pDict[ParameterNames.FirstMolarsNarrowing].ConvertValue<float>(),
                    pDict[ParameterNames.DentistPointsSum].ConvertValue<float>(),
                    pDict[ParameterNames.SagittalSlit].ConvertValue<float>(),
                    pDict[ParameterNames.VerticalDysocclusion].ConvertValue<float>(),
                    pDict[ParameterNames.LessIncisorOverlap].ConvertValue<float>(),
                    pDict[ParameterNames.ContactIncisorOverlapWithoutInjury].ConvertValue<float>(),
                    pDict[ParameterNames.ContactIncisorOverlapWithInjury].ConvertValue<float>(),
                    pDict[ParameterNames.LowerJawForwardDisplacement].ConvertValue<float>(),
                    pDict[ParameterNames.LowerJawBackwardDisplacement].ConvertValue<float>(),
                    pDict[ParameterNames.LowerJawSideDisplacement].ConvertValue<float>(),
                    pDict[ParameterNames.DentitionLengthReductionByTooth].ConvertValue<float>(),
                    pDict[ParameterNames.DentitionLengthReductionByTooths].ConvertValue<float>(),
                    age,
                    pDict[ParameterNames.ScoreAfterTreatment].ConvertValue<float>(),
                    pDict[ParameterNames.TreatmentDuration].ConvertValue<float>(),
                    pDict[ParameterNames.TreatmentSteps].ConvertValue<float>(),
                    pDict[ParameterNames.TreatmentApparatuesCount].ConvertValue<float>(),
                };
            }

            IPredictRequest request = new PredictRequest() { Id = model.Id, Version = model.Version, Input = inputArgs};

            var responce = await _webRequester.SendRequest($"{_modelsServerUrl}/models/predict/", "POST", Newtonsoft.Json.JsonConvert.SerializeObject(request));
            if (!responce.IsSuccessStatusCode)
                throw new ExecuteCommandException($"{responce.StatusCode}:{responce.ReasonPhrase}");
            else
            {
                var res = await _webRequester.DeserializeBody<float[]>(responce);
                return (int)res.First();
            }
        };


        private ModelKey GetModelByAge(float age)
        {
            if (age >= 3 && age <= 5)
                return _settings.TempModelSettings.Dentist_3_5;
            if (age >= 6 && age <= 9)
                return _settings.TempModelSettings.Dentist_6_9;
            else if (age >= 9 && age <= 12)
                return _settings.TempModelSettings.Dentist_10_12;
            else return null;
        }
    }
}
