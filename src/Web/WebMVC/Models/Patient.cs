using Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebMVC.Models
{
    public class Patient : IPatient
    {

        public Patient() { }

        public int Id { get ; set ; }

        [Display(Name="ФИО")]
        [Required(ErrorMessage = "Не указано ФИО")]
        public string Name { get ; set ; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [DateSet(ErrorMessage = "Не указана дата рождения")]
        [Display(Name = "Дата рождения")]
        public DateTime Birthday { get ; set ; }

        [Display(Name = "Номер истории болезни")]
        [Range(1, int.MaxValue, ErrorMessage = "Значение должно быть положительным числом")]
        [Required(ErrorMessage = "Не указан идентификатор пациента")]
        public int MedicalHistoryNumber { get ; set ; }

        [Display(Name = "Пол")]
        [GenderSet(ErrorMessage = "Не указан пол")]
        public GenderEnum Gender { get; set; }


        [Display(Name = "Вид лечения")]
        [GenderSet(ErrorMessage = "Не указан вид лечения")]
        public TreatmentType TreatmentType { get; set; }
    }
}
