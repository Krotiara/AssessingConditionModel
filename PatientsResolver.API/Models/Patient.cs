using Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace PatientsResolver.API.Models
{
    public class Patient : IPatient
    {

        public Patient()
        {

        }

        [NotNull]
        [Key]
        [Column("Id")]
        public long Id { get; set; }

        [NotNull]
        [Column("Name")]
        public string Name { get; set; }

        [NotNull]
        [Column("Birthday")]
        public DateTime Birthday { get; set; }

        [NotNull]
        [Column("MedicalHistoryNumber")]
        public long MedicalHistoryNumber { get; set; }
    }
}
