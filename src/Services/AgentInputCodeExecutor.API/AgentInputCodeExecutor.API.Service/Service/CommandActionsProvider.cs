using AgentInputCodeExecutor.API.Entities;
using AgentInputCodeExecutor.API.Interfaces;
using Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentInputCodeExecutor.API.Service.Service
{
    public class CommandActionsProvider : ICommandActionsProvider
    {

        private readonly IMediator mediator;
        private readonly IWebRequester webRequester;
        private readonly Dictionary<string, Delegate> delegates;
        private readonly string patientsResolverApiUrl;
        private readonly string bioAgeApiUrl;

        public CommandActionsProvider(IMediator mediator, IWebRequester webRequester)
        {
            this.webRequester = webRequester;
            this.mediator = mediator;
            patientsResolverApiUrl = Environment.GetEnvironmentVariable("PATIENTRESOLVER_API_URL");
            bioAgeApiUrl = Environment.GetEnvironmentVariable("BIO_AGE_API_URL"); //TODO - в отдельный сервис
            delegates = new Dictionary<string, Delegate>();
            InitDelegates();
        }

        public Delegate? GetDelegateByCommandNameWithoutParams(string commandName)
        {
            return delegates.ContainsKey(commandName) ? delegates[commandName] : null;
        }


        private void InitDelegates()
        {
            
            // TODO Список методов нужно вынести в отдельное место.
            delegates["GetLatestPatientParams"] = async (DateTime startTimestamp, DateTime endTimestamp, int patientId) =>
            {
                string body = Newtonsoft.Json.JsonConvert.SerializeObject(new DateTime[2] { startTimestamp, endTimestamp });
                string url = $"{patientsResolverApiUrl}/patientsApi/latestPatientParameters/{patientId}";
                return await webRequester
                  .GetResponse<IList<PatientParameter>>(url, "POST", body);
            };

            delegates["GetAge"] = async (List<PatientParameter> parameters) =>
            {
                IPatientParameter ageParam = parameters.FirstOrDefault(x => x.ParameterName == ParameterNames.Age);
                if (ageParam == null)
                    throw new NotImplementedException(); //TODO - обработка такого случая.
                double age = double.Parse(ageParam.Value);
                return age;
            };

        }
    }
}
