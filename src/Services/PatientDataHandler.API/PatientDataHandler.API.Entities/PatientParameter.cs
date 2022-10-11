using Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace PatientDataHandler.API.Entities
{
    [Table("PatientParameters")]
    public class PatientParameter : IPatientParameter
    {
        public PatientParameter()
        {

        }

        public PatientParameter(Parameters parameterName)
        {
            ParameterName = parameterName;
            NameTextDescription = parameterName.GetDisplayAttributeValue();
        }

        [NotNull]
        [Column("Id")]
        [Key]
        public int Id { get; set; }


        [NotNull]
        [Column("PatientDataId")]
        public int PatientDataId { get; set; } 


        [NotNull]
        [Required]
        public int PatientId { get ; set ; }

        [NotNull]
        [Required]
        public DateTime Timestamp { get ; set ; }

        [NotNull]
        [Required]
        public string NameTextDescription { get ; set ; }

        [NotNull]
        [Required]
        public string Value { get ; set ; }
       
        public string DynamicValue { get ; set ; }
        
        [NotNull]
        [Required]
        public int PositiveDynamicCoef { get ; set ; }
        public Parameters ParameterName { get; set; }
    }
}
