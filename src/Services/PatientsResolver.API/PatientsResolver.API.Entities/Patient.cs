using Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace PatientsResolver.API.Entities
{
    [Table("Patients")]
    public class Patient : IPatient
    {

        public Patient()
        {

        }

        [NotNull]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [NotNull]
        [Required]
        [Column("Name")]
        public string Name { get; set; }

        [NotNull]
        [Required]
        [Column("Birthday")]
        public DateTime Birthday { get; set; }

        [NotNull]
        [Required]
        [Key]
        [Column("MedicalHistoryNumber")]
        public int MedicalHistoryNumber { get; set; }
    }
}
