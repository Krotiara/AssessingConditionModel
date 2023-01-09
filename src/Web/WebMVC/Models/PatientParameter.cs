using Interfaces;
using System.ComponentModel.DataAnnotations;

namespace WebMVC.Models
{
    public class PatientParameter : IPatientParameter
    {
        public PatientParameter() { }

        public int Id { get; set; }
        public int InfluenceId { get; set; }

        [Display(Name="Идентификатор пациента")]
        public int PatientId { get; set; }
        [Display(Name = "Временная отметка")]
#warning Нужна валидация
        public DateTime Timestamp { get; set; }
        [Display(Name = "Имя показателя")]
        [ParamNameSet(ErrorMessage = "Не указано наименование параметра")]
        public ParameterNames ParameterName { get; set; }
        public string NameTextDescription { get; set; }
        [Display(Name = "Значение")]
        [Required(ErrorMessage = "Не указано значение параметра")]
        public string Value { get; set; }
        [Display(Name = "Вторичный")]
        public bool IsDynamic { get; set; }
        public int PositiveDynamicCoef { get; set; }
    }
}
