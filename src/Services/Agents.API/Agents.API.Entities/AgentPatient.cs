using Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Entities
{
    [Table("PatientAgents")]
    public class AgentPatient : IAgent
    {
        private IWebRequester webRequester;
       
        public AgentPatient() 
        {
            InitStateDiagram();
        }

        [NotNull]
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [NotNull]
        [Required]
        [Column("PatientId")]
        public int PatientId { get; set; }

        [NotNull]
        [Required]
        [Column("Name")]
        public string Name { get ; set ; }

        [NotMapped]
        public StateDiagram StateDiagram { get ; set ; }

        [NotMapped]
        public List<IAgent> Connections { get ; set ; }

        [NotMapped]
        public double CurrentAge { get; set; }

        [NotMapped]
        public double CurrentBioAge { get; set; }
        

        public void InitStateDiagram()
        {
            StateDiagram = new StateDiagram(DetermineState);
            foreach (AgentBioAgeStates state in AgentBioAgeStates.GetValues(typeof(AgentBioAgeStates)))
            {
                StateDiagram.AddState(state.GetDisplayAttributeValue());
            }
        }


        private async Task<State> DetermineState(IAgentDetermineStateProperties determineStateProperties)
        {
            IList<PatientParameter> patientParams = await GetLatestPatientParameters(determineStateProperties);
            if (patientParams == null)
                throw new DetermineStateException($"No patient parameters to determine state. Patient id = {PatientId}");
            PatientParameter ageParam = patientParams.FirstOrDefault(x => x.ParameterName == ParameterNames.Age);
            if(ageParam == null)
                throw new DetermineStateException($"No patient age in input patient parameters. Patient id = {PatientId}");

            double age = double.Parse(ageParam.Value);
            double bioAge = await GetBioAge(patientParams);
            double ageDelta = bioAge-age;

            CurrentAge = age;
            CurrentBioAge = bioAge;

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

            return StateDiagram.GetState(rang.GetDisplayAttributeValue());
        }
       

        private async Task<IList<PatientParameter>> GetLatestPatientParameters(IAgentDetermineStateProperties determineStateProperties)
        {
            try
            {
                DateTime startTimestamp = (determineStateProperties.StartTimestamp == null ? DateTime.MinValue : (DateTime)determineStateProperties.StartTimestamp);
                DateTime endTimestamp = (determineStateProperties.EndTimestamp == null ? DateTime.MaxValue : (DateTime)determineStateProperties.EndTimestamp);
                string body = Newtonsoft.Json.JsonConvert.SerializeObject(new DateTime[2] { startTimestamp, endTimestamp });
                string url = $"https://host.docker.internal:8004/latestPatientParameters/{PatientId}";
                return await webRequester
                  .GetResponse<IList<PatientParameter>>(url, "POST", body);
            }
            catch (GetWebResponceException ex)
            {
                return null; //TODO custom exception
            }
            catch (Exception unexpectedEx)
            {
                //TODO
                throw new NotImplementedException();
            }
        }


        private async Task<double> GetBioAge(IList<PatientParameter> patientParams)
        {
            try
            {
                BioAgeCalculationParameters calculationParameters = new BioAgeCalculationParameters()
                {
                    CalculationType = BioAgeCalculationType.ByFunctionalParameters,
                    Parameters = patientParams.ToDictionary(entry => entry.ParameterName, entry => entry)
                };

                string requestBody = Newtonsoft.Json.JsonConvert.SerializeObject(calculationParameters);
                string url = $"https://host.docker.internal:8006/bioAge/";
                return await webRequester.GetResponse<double>(url, "PUT", requestBody);
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
        }

  
        public void ProcessPrivateTransitions()
        {
            throw new NotImplementedException();
        }


        public void InitWebRequester(IWebRequester webRequester)
        {
            this.webRequester = webRequester;
        }
    }
}
