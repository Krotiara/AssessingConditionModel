using Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace PatientsResolver.API.Entities
{
    //TODO упростить в связи с введением Parameter в Interfaces.

    [Table("PatientParameters")]
    public class PatientParameter : IPatientParameter
    {
        public PatientParameter()
        {

        }

        [NotNull]
        [Column("Id")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [NotNull]
        [Column("InfluenceId")]
        public int InfluenceId { get; set; }

        [NotNull]
        [Required]
        [Column("PatientId")]
        public int PatientId { get; set; }

        [NotNull]
        [Required]
        [Column("PatientAffiliation")]
        public string PatientAffiliation { get; set; }

        [NotNull]
        [Required]
        [Column("Timestamp")]
        public DateTime Timestamp { get; set; }

        [NotNull]
        [Required]
        [Column("Name")]
        public string Name { get; set; }

        [NotNull]
        [Required]
        [Column("Value")]
        public string Value { get; set; }

        [NotNull]
        [Required]
        [Column("IsDynamic")]
        public bool IsDynamic { get; set; }
    }
}
