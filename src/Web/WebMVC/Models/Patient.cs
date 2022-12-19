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
        public string Name { get ; set ; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [Display(Name = "Дата рождения")]
        public DateTime Birthday { get ; set ; }
        [Display(Name = "Номер истории болезни")]
        public int MedicalHistoryNumber { get ; set ; }
        [Display(Name = "Пол")]
        public GenderEnum Gender { get; set; }
    }
}
