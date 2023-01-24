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
        [Range(1, int.MaxValue, ErrorMessage = "Значение должно быть положительным числом")]
        [Required(ErrorMessage = "Не указан идентификатор пациента")]
        public int PatientId { get; set; }

        [Display(Name = "Начало воздействия")]
        [DataType(DataType.Date)]
#warning Нужна валидация
        public DateTime StartTimestamp { get; set; }

        [Display(Name = "Окончание воздействия")]
        [DataType(DataType.Date)]
#warning Нужна валидация
        public DateTime EndTimestamp { get; set; }

        [Display(Name = "Тип воздействия")]
        [InfluenceTypeSet(ErrorMessage = "Не указан тип воздействия")]
        public InfluenceTypes InfluenceType { get; set; }

        [Display(Name = "Наименование")]
        [Required(ErrorMessage = "Не указано наименование")]
        public string MedicineName { get; set; }

        [Display(Name = "Параметры")]
        [InfluenceParamsSet(ErrorMessage = "Не введены показатели пациента")]
        public List<PatientParameter> Parameters { get; set; }
    }
}
