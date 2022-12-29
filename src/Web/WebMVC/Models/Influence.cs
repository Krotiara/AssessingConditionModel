using Interfaces;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;

namespace WebMVC.Models
{
    public class Influence : IInfluence<Patient, PatientParameter>
    {
        public Influence()
        {
            StartParameters = new ConcurrentDictionary<ParameterNames, PatientParameter>();
            DynamicParameters = new ConcurrentDictionary<ParameterNames, PatientParameter>();
        }

        [Display(Name = "Идентификатор воздействия")]
        public int Id { get ; set ; }

        [Display(Name ="Идентификатор пациента")]
        public int PatientId { get ; set ; }
        public Patient Patient { get ; set ; }

        [Display(Name = "Дата начала")]
        public DateTime StartTimestamp { get ; set ; }

        [Display(Name = "Дата окончания")]
        public DateTime EndTimestamp { get ; set ; }

        [Display(Name = "Тип воздействия")]
        public InfluenceTypes InfluenceType { get ; set ; }

        [Display(Name = "Наименование")]
        public string MedicineName { get ; set ; }
        public ConcurrentDictionary<ParameterNames, PatientParameter> StartParameters { get ; set ; }
        public ConcurrentDictionary<ParameterNames, PatientParameter> DynamicParameters { get ; set ; }
    }
}
