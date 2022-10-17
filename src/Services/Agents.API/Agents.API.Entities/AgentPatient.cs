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
        

        public void InitStateDiagram()
        {
            StateDiagram = new StateDiagram(DetermineState);
            foreach (AgentBioAgeStates state in AgentBioAgeStates.GetValues(typeof(AgentBioAgeStates)))
            {
                StateDiagram.AddState(state.GetDisplayAttributeValue());
            }
        }


        private async Task<State> DetermineState()
        {
            //TODO получение последнего значения параметров
            IList<PatientData> patientDatas = await GetPatientData(DateTime.MinValue, DateTime.MaxValue);
            if (patientDatas == null)
                throw new DetermineStateException($"No patient data to determine state. Patient id = {PatientId}");

            //TODO get bioAge from webRequest

            double bioAge = await GetBioAge(patientDatas.Last());

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


        private async Task<IList<PatientData>> GetPatientData(DateTime startTime, DateTime endTime)
        {
            //TODO указание времени.
            //TODO IList<IPatientData<IPatientParameter, IPatient, IInfluence>> - выглядит перегруженно.
            try
            {
                string url = $"https://host.docker.internal:8004/patientsData/{PatientId}";
                return await webRequester
                    .GetResponse<IList<PatientData>>(url, "GET");
            }
            catch(GetWebResponceException ex)
            {
                return null; //TODO custom exception
            }
            catch(Exception unexpectedEx)
            {
                //TODO
                throw new NotImplementedException();
            }
        }


        private async Task<double> GetBioAge(PatientData patientData)
        {
            try
            {
                BioAgeCalculationParameters calculationParameters = new BioAgeCalculationParameters()
                {
                    CalculationType = BioAgeCalculationType.ByFunctionalParameters,
                    Parameters = patientData.Parameters.ToDictionary(entry => entry.Key, entry => entry.Value)
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


        #region test stuff
        private async Task TryAll(string hostName)
        {
            string url = $"https://patientsresolver.api:443/patientsData/{PatientId}";
            //string url = $"https://localhost:8003/patientsData/{PatientId}";

            IList<IPatientData<IPatientParameter, IPatient, IInfluence>> data =
                new List<IPatientData<IPatientParameter, IPatient, IInfluence>>();

            try
            {
                url = $"https://{hostName}:443/patientsData/{PatientId}";
                data = await webRequester
                    .GetResponse<IList<IPatientData<IPatientParameter, IPatient, IInfluence>>>(
                    url, "GET");
            }
            catch (Exception ex)
            {

            }
            try
            {
                url = $"http://{hostName}:80/patientsData/{PatientId}";
                data = await webRequester
                   .GetResponse<IList<IPatientData<IPatientParameter, IPatient, IInfluence>>>(
                   url, "GET");
            }
            catch (Exception ex)
            {

            }
            try
            {
                url = $"https://{hostName}:8004/patientsData/{PatientId}";
                data = await webRequester
                   .GetResponse<IList<IPatientData<IPatientParameter, IPatient, IInfluence>>>(
                   url, "GET");
            }
            catch (Exception ex)
            {

            }
            try
            {
                url = $"http://{hostName}:8003/patientsData/{PatientId}";
                data = await webRequester
                   .GetResponse<IList<IPatientData<IPatientParameter, IPatient, IInfluence>>>(
                   url, "GET");
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

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
