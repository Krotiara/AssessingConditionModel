using Interfaces;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

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
        [HiddenInput(DisplayValue = false)]
        public int Id { get ; set ; }

        [Display(Name ="Идентификатор пациента")]
        [Range(1, int.MaxValue, ErrorMessage = "Значение должно быть положительным числом")]
        [Required(ErrorMessage = "Не указан идентификатор пациента")]
        public int PatientId { get ; set ; }
        public Patient Patient { get ; set ; }

        [Display(Name = "Дата начала")]
        [DataType(DataType.Date)]
#warning Нужна валидация
        public DateTime StartTimestamp { get ; set ; }

        [Display(Name = "Дата окончания")]
        [DataType(DataType.Date)]
#warning Нужна валидация
        public DateTime EndTimestamp { get ; set ; }

        [Display(Name = "Тип воздействия")]
        [InfluenceTypeSet(ErrorMessage = "Не указан тип воздействия")]
        public InfluenceTypes InfluenceType { get ; set ; }

        [Display(Name = "Наименование")]
        [Required(ErrorMessage = "Не указано наименование")]
        public string MedicineName { get ; set ; }
        public ConcurrentDictionary<ParameterNames, PatientParameter> StartParameters { get ; set ; }
        public ConcurrentDictionary<ParameterNames, PatientParameter> DynamicParameters { get ; set ; }
        public string MedicalOrganization { get; set; }
    }
}
