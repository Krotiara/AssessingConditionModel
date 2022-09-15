using Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace PatientsResolver.API.Models
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

        public IPatient Patient { get; set; }

        [NotNull]
        [Column("InfluenceId")]
        public int InfluenceId { get ; set ; }
        
        public IList<PatientParameter> Parameters { get ; set ; }

        [NotMapped]
        IList<IPatientParameter> IPatientData.Parameters
        {
            get { return (IList<IPatientParameter>)Parameters; }
            set { Parameters = value as IList<PatientParameter>; }
        }

    }
}
