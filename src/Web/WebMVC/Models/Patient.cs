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
        public string Name { get ; set ; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Birthday { get ; set ; }
        public int MedicalHistoryNumber { get ; set ; }

        public GenderEnum Gender { get; set; }
    }
}
