using Interfaces;
using System.ComponentModel.DataAnnotations;

namespace WebMVC.Models
{
    public class InfluenceViewFormat
    {
        public InfluenceViewFormat()
        {
            Parameters = new List<PatientParameter>();
        }

        //public InfluenceViewFormat(Influence influence)
        //{
        //    PatientId = influence.PatientId;
        //    StartTimestamp = influence.StartTimestamp;
        //    EndTimestamp = influence.EndTimestamp;
        //    InfluenceType = influence.InfluenceType;
        //    MedicineName = influence.MedicineName;
        //    Parameters = new List<PatientParameter>();
        //    Parameters.AddRange(influence.StartParameters.Values);
        //    Parameters.AddRange(influence.DynamicParameters.Values);
        //}

        [Display(Name="Идентификатор пациента")]
        public int PatientId { get; set; }

        [Display(Name = "Начало воздействия")]
        public DateTime StartTimestamp { get; set; }

        [Display(Name = "Окончание воздействия")]
        public DateTime EndTimestamp { get; set; }

        [Display(Name = "Тип")]
        public InfluenceTypes InfluenceType { get; set; }

        [Display(Name = "Наименование")]
        public string MedicineName { get; set; }

        [Display(Name = "Параметры")]
        public List<PatientParameter> Parameters { get; set; }
    }
}
