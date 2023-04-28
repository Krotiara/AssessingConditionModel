using Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace PatientDataHandler.API.Entities
{
    //TODO упростить в связи с введением Parameter в Interfaces.

    [Table("PatientParameters")]
    public class PatientParameter : IPatientParameter
    {
        public PatientParameter()
        {

        }

        public PatientParameter(string parameterName)
        {
            Name = parameterName;
        }

        public int Id { get; set; }
        public int InfluenceId { get; set; }
        public int PatientId { get; set; }
        public string PatientAffiliation { get; set; }
        public DateTime Timestamp { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public bool IsDynamic { get; set; }
    }
}
