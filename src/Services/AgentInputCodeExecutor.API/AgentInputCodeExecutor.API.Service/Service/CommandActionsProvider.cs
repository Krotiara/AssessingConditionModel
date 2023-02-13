using AgentInputCodeExecutor.API.Entities;
using AgentInputCodeExecutor.API.Interfaces;
using Interfaces;
using Interfaces.DynamicAgent;
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
        private readonly Dictionary<SystemCommands, Delegate> delegates;
        private readonly string patientsResolverApiUrl;
        private readonly string bioAgeApiUrl;

        public CommandActionsProvider(IMediator mediator, IWebRequester webRequester)
        {
            this.webRequester = webRequester;
            this.mediator = mediator;
            patientsResolverApiUrl = Environment.GetEnvironmentVariable("PATIENTRESOLVER_API_URL");
            bioAgeApiUrl = Environment.GetEnvironmentVariable("BIO_AGE_API_URL"); //TODO - в отдельный сервис
            delegates = new Dictionary<SystemCommands, Delegate>();
            InitDelegates();
        }

        public Delegate? GetDelegateByCommandNameWithoutParams(SystemCommands commandName)
        {
            return delegates.ContainsKey(commandName) ? delegates[commandName] : null;
        }


        private void InitDelegates()
        {
            
            // TODO Список методов нужно вынести в отдельное место.
            delegates[SystemCommands.GetLatestPatientParameters] = async (DateTime startTimestamp, DateTime endTimestamp, int patientId) =>
            {
                string body = Newtonsoft.Json.JsonConvert.SerializeObject(new DateTime[2] { startTimestamp, endTimestamp });
                string url = $"{patientsResolverApiUrl}/patientsApi/latestPatientParameters/{patientId}";
                return await webRequester
                  .GetResponse<IList<PatientParameter>>(url, "POST", body);
            };

            delegates[SystemCommands.GetAge] = async (List<PatientParameter> parameters) =>
            {
                IPatientParameter ageParam = parameters.FirstOrDefault(x => x.ParameterName == ParameterNames.Age);
                if (ageParam == null)
                    throw new NotImplementedException(); //TODO - обработка такого случая.
                long age = long.Parse(ageParam.Value);
                return age;
            };

            delegates[SystemCommands.GetBioage] = async (List<PatientParameter> parameters) =>
            {
                try
                {
                    BioAgeCalculationParameters calculationParameters = new BioAgeCalculationParameters()
                    {
                        CalculationType = BioAgeCalculationType.ByFunctionalParameters,
                        Parameters = parameters.ToDictionary(entry => entry.ParameterName, entry => entry)
                    };

                    string requestBody = Newtonsoft.Json.JsonConvert.SerializeObject(calculationParameters);
                    string url = $"{bioAgeApiUrl}/bioAge/";
                    return await webRequester.GetResponse<long>(url, "PUT", requestBody);
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

            delegates[SystemCommands.GetAgeRangBy] = async (long age, long bioAge) =>
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

            //TODO Добавить meta инфу для этого действия
            delegates[SystemCommands.GetInfluences] = async (DateTime startTimestamp, DateTime endTimestamp, int patientId) =>
            {
                string body = Newtonsoft.Json.JsonConvert.SerializeObject(new DateTime[2] { startTimestamp, endTimestamp });
                string url = $"{patientsResolverApiUrl}/patientsApi/influences/{patientId}";
                return await webRequester.GetResponse<IList<PatientParameter>>(url, "POST", body);
            };

            //TODO Добавить meta инфу для этого действия
            delegates[SystemCommands.GetAllInfluences] = async (DateTime startTimestamp, DateTime endTimestamp) =>
            {
                string url = $"{patientsResolverApiUrl}/patientsApi/influences/";
                string body = Newtonsoft.Json.JsonConvert.SerializeObject(new DateTime[2] { startTimestamp, endTimestamp });
                return await webRequester.GetResponse<List<Influence>>(url, "POST", body);
            };

            //TODO Добавить meta инфу для этого действия
            delegates[SystemCommands.GetAgingDynamics] = async (DateTime startTimestamp, DateTime endTimestamp, 
                IDynamicAgent patientAgent, IList<Influence> influences) =>
            {
                //try
                //{
                    List<IAgingDynamics<AgingState>> res = new List<IAgingDynamics<AgingState>>();
                    foreach (Influence influence in influences)
                    {
                        AgingDynamics agingDynamics = new AgingDynamics()
                        {
                            StartTimestamp = influence.StartTimestamp,
                            EndTimestamp = influence.EndTimestamp,
                            InfluenceType = influence.InfluenceType,
                            MedicineName = influence.MedicineName,
                            PatientId = patientAgent.ObservedId
                        };

                        agingDynamics.AgentStateInInfluenceStart = await CalcAgentStateInInfluenceStartAsync(patientAgent, influence);
                        agingDynamics.AgentStateInInfluenceEnd = await CalcAgentStateInInfluenceEndAsync(patientAgent, influence);

                        res.Add(agingDynamics);
                    }
                    return res;
                //}
                //catch (AgentNotFoundException ex)
                //{
                //    throw new GetAgingDynamicsException($"Агент для пациента с id = {request.PatientId} не был найден", ex);
                //}
                //catch (GetAgentException ex)
                //{
                //    throw new GetAgingDynamicsException($"Не удалось обновить состояние агента для пациента с id = {request.PatientId}", ex);
                //}
                //catch (Exception ex)
                //{
                //    throw new GetAgingDynamicsException($"Unexpected error", ex);
                //}
            };


        }


        //TODO - продумать смену параметров для моделирования и последующий вызов.
        //TODO - вариант решения - разбить это действие на ряд атомарных команд, а затем просто передавать на исполнение.
        private async Task<AgingState> CalcAgentStateInInfluenceStartAsync(IDynamicAgent agent, Influence influence)
        {
            throw new NotImplementedException();
            //await agent.StateDiagram.UpdateStateAsync(new AgentDetermineStateProperties()
            //{
            //    Timestamp = influence.StartTimestamp
            //});

            //return new AgingState()
            //{
            //    PatientId = agent.PatientId,
            //    Age = agent.CurrentAge,
            //    BioAge = agent.CurrentBioAge,
            //    BioAgeState = agent.CurrentAgeRang,
            //    Timestamp = influence.StartTimestamp
            //};
        }


        private async Task<AgingState> CalcAgentStateInInfluenceEndAsync(IDynamicAgent agent, Influence influence)
        {
            throw new NotImplementedException();
            //await agent.StateDiagram.UpdateStateAsync(new AgentDetermineStateProperties()
            //{
            //    Timestamp = influence.EndTimestamp
            //});

            //return new AgingState()
            //{
            //    PatientId = agent.PatientId,
            //    Age = agent.CurrentAge,
            //    BioAge = agent.CurrentBioAge,
            //    BioAgeState = agent.CurrentAgeRang,
            //    Timestamp = influence.EndTimestamp
            //};
        }
    }
}
