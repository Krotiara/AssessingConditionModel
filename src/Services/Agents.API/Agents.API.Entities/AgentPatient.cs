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
       
        public AgentPatient() { }

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
        

        public void InitStateDiagram()
        {
            StateDiagram = new StateDiagram(DetermineState);
            foreach (AgentBioAgeStates state in AgentBioAgeStates.GetValues(typeof(AgentBioAgeStates)))
            {
                StateDiagram.AddState(state.GetDisplayAttributeValue());
            }
            StateDiagram.UpdateStateAsync();
        }


        private async Task<State> DetermineState()
        {
            //TODO get patient params
            //https://localhost:60571/patientsData/2
            var patientDatas = await GetPatientData(DateTime.MinValue, DateTime.MaxValue);
            //TODO get bioAge from webRequest
            // calc delta
            //TODO check ненулевые значения.
            //double ageDelta = AgeDelta;
            double ageDelta = 0;
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


        private async Task<IList<IPatientData<IPatientParameter,IPatient,IInfluence>>> GetPatientData(DateTime startTime, DateTime endTime)
        {
            //TODO указание времени.
            //TODO IList<IPatientData<IPatientParameter, IPatient, IInfluence>> - выглядит перегруженно.
            //string url = $"https://host.docker.internal:8003/patientsData/{PatientId}";
           
            string url = $"https://patientsresolver.api:8003/patientsData/{PatientId}";
            //string url = $"https://localhost:8003/patientsData/{PatientId}";
            var a = await webRequester
                .GetResponse<IList<IPatientData<IPatientParameter, IPatient, IInfluence>>>(
                url, "GET");
            return a;
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
