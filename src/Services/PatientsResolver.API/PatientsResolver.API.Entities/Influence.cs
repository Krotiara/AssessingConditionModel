using Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsResolver.API.Entities
{
    public class Influence : IInfluence
    {

        public Influence()
        {

        }

        [Key]
        [NotNull]
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
        public string MedicineName { get ; set ; }

        [NotNull]
        [Column("PatientId")]
        public int PatientId { get; set; }
    }
}
