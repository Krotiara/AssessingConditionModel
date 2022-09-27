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

       
        [ForeignKey(nameof(PatientId))]
        public Patient Patient { get; set; }

      
        [ForeignKey(nameof(InfluenceId))]
        public Influence Influence { get; set; }

        [NotMapped]
        IPatient IPatientData.Patient
        {
            get { return Patient; }
            set { Patient = (Patient)value; }
        }

        [NotMapped]
        IInfluence IPatientData.Influence
        {
            get { return Influence; }
            set { Influence = (Influence)value; }
        }

        [NotMapped]
        IList<IPatientParameter> IPatientData.Parameters
        {
            get { return Parameters.Select(x=>x as IPatientParameter).ToList(); }
            set { Parameters = value.Select(x=>x as PatientParameter).ToList(); }
        }

    }
}
