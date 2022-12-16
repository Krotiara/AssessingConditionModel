using Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsResolver.API.Entities
{
    public class Influence : IInfluence<Patient,PatientParameter>
    {

        public Influence()
        {
            StartParameters = new ConcurrentDictionary<ParameterNames, PatientParameter>();
            DynamicParameters = new ConcurrentDictionary<ParameterNames, PatientParameter>();
        }

        [Key]
        [NotNull]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get ; set ; }

        [NotNull]
        [Column("StartTimestamp")]
        public DateTime StartTimestamp { get ; set ; }

        [NotNull]
        [Column("EndTimestamp")]
        public DateTime EndTimestamp { get ; set ; }

        [NotNull]
        [Column("InfluenceType")]
        public InfluenceTypes InfluenceType { get ; set ; }

        [NotNull]
        [Column("MedicineName")]
        //[Required(AllowEmptyStrings = false)] не работает, не вызывается dbValidationError
        public string MedicineName { get ; set ; }

        [NotNull]
        [Column("PatientId")]
        public int PatientId { get; set; }

        [ForeignKey(nameof(PatientId))]
        public Patient Patient { get; set; }

        [NotMapped]
        public ConcurrentDictionary<ParameterNames, PatientParameter> StartParameters { get; set; }

        [NotMapped]
        public ConcurrentDictionary<ParameterNames, PatientParameter> DynamicParameters { get; set; }
    }
}
