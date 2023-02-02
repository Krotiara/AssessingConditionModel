using AgentInputCodeExecutor.API.Entities;
using AgentInputCodeExecutor.API.Interfaces;
using AgentInputCodeExecutor.API.Service.Command;
using AgentInputCodeExecutor.API.Service.Queue;
using Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AgentInputCodeExecutor.API.Service.Service
{
    public class CodeResolveService : ICodeResolveService
    {

        private readonly IMediator mediator;
        private readonly IWebRequester webRequester;
        //TODO вынести получение в отдельный класс.
        private readonly string patientsResolverApiUrl;
        private readonly string bioAgeApiUrl;

        private readonly Dictionary<string, Delegate> delegates;

        public CodeResolveService(IMediator mediator, IWebRequester webRequester)
        {
            this.mediator = mediator;
            this.webRequester = webRequester;
            patientsResolverApiUrl = Environment.GetEnvironmentVariable("PATIENTRESOLVER_API_URL");
            bioAgeApiUrl = Environment.GetEnvironmentVariable("BIO_AGE_API_URL");
            delegates = new Dictionary<string, Delegate>();
            InitDelegates();  
        }

        public async Task<(ICommandArgsTypesMeta, Delegate)> ResolveCommandAction(ICommand command)
        {
            string commandName = await mediator.Send(new GetCommandNameCommand(command));

            if (commandName == null)
            {
                if (command.CommandType != CommandType.Assigning)
                    throw new ResolveCommandActionException($"Выполнение действия без команды допустимо только для присвоения значения");

                //Создать делегат для вычисления/присвоения значения.
                throw new NotImplementedException();
            }
            else
            {
                ICommandArgsTypesMeta meta = await mediator.Send(new GetCommandTypesMetaQueue(commandName));
                if (!delegates.ContainsKey(commandName))
                    throw new ResolveCommandActionException($"Не удалось разрешить действие для команды {commandName}");
                return (meta, delegates[commandName]);
            }
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

        }
    }
}
