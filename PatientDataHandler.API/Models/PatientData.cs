using Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace PatientDataHandler.API.Models
{
    public class PatientData : IPatientData
    {
        public PatientData()
        {

        }

        [Key]
        [NotNull]
        [Column("Id")]
        public int Id { get; set; }

        [NotNull]
        [Column("Timestamp")]
        public DateTime Timestamp { get; set; }

        [NotNull]
        [Column("PatientId")]
        public int PatientId { get ; set ; }
        
        [NotNull]
        [Column("InfluenceId")]
        public int InfluenceId { get ; set ; }
        
        public IList<IPatientParameter> Parameters { get ; set ; }
    }
}
