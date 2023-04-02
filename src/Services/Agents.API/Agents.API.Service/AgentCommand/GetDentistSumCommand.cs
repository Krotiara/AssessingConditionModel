using Agents.API.Entities;
using Interfaces;
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

        public GetDentistSumCommand(IWebRequester webRequester, EnvSettings settings)
        {
            _webRequester = webRequester;
            _modelsServerUrl = settings.ModelsApiUrl;
        }

        public Delegate Command => async (Dictionary<ParameterNames, PatientParameter> pDict) =>
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
                    pDict[ParameterNames.LowerJawForwardDisplacement].ConvertValue<float>(),
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
            float[] res = (await _webRequester.GetResponse<float[]>($"{_modelsServerUrl}/models/predict/",
                "POST", Newtonsoft.Json.JsonConvert.SerializeObject(request)));
            return (int)res.First();

        };
    }
}
