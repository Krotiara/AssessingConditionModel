using Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfluenceCalculator.API.Models
{
    [Table("InfluenceResults")]
    public class InfluenceResult : IInfluenceResult
    {
        public InfluenceResult() { }

        public int Id { get; set; }

        public IInfluence Influence { get; set; }

        public double InfluenceEffectiveness { get; set; }

        public int PatientId { get; set; }
    }
}
