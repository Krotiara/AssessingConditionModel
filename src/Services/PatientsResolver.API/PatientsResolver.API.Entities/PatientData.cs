using Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace PatientsResolver.API.Entities
{
    [Table("PatientDatas")]
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
        
        public IList<PatientParameter> Parameters { get ; set ; }

        [NotMapped]
        IList<IPatientParameter> IPatientData.Parameters
        {
            get { return (IList<IPatientParameter>)Parameters; }
            set { Parameters = value as IList<PatientParameter>; }
        }

        public Patient Patient { get; set; }

        [NotMapped]
        IPatient IPatientData.Patient
        {
            get { return Patient; }
            set { Patient = (Patient)value; }
        }

        [NotMapped]
        IInfluence IPatientData.Influence {
            get { return Influence;  }
            set { Influence = (Influence)value; }
        }

        public Influence Influence { get; set; }
    }
}
