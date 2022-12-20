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
